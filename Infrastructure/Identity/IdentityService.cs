using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Requests.Identity.Commands.RefreshToken;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParams;

        public IdentityService(
            ApplicationDbContext context,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            UserManager<ApplicationUser> userManager,
            TokenValidationParameters tokenValidationParams)
        {
            _context = context;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParams = tokenValidationParams;
        }

        public async Task<Result> CreateUserAsync(string username, string email, string password)
        {
            // change based on your custom user properties
            var user = new ApplicationUser
            {
                UserName = username,
                Nickname = "Brotherman" + username,
                Email = email,
            };

            var result = await _userManager.CreateAsync(user, password);

            return result.ToApplicationResult();
        }

        public async Task<bool> EmailNotAvailableAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public async Task<bool> EmailPasswordNotValidAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return !await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<AuthResult> GenerateJwtTokens(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(user.GenerateClaims()),
                Expires = DateTime.UtcNow.AddSeconds(500),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };

            var createdToken = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(createdToken);
            var refreshToken = await CreateRefreshTokenAsync(user, createdToken);

            return AuthResult.Successful(jwtToken, refreshToken);
        }

        private async Task<string> CreateRefreshTokenAsync(ApplicationUser user, SecurityToken token)
        {

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Value = CommonExtensions.GenerateRandomString(35) + Guid.NewGuid().ToString()
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Value;
        }

        public async Task<AuthResult> ValidateAndCreateTokensAsync(TokenRequestDto tokenRequest)
        {
            // Validation 1: validate token
            var jwtTokenClaimsPrincipal = tokenRequest.JwtToken.GetClaimsPrincipal(_tokenValidationParams);

            if (jwtTokenClaimsPrincipal is null)
            {
                return AuthResult.Failed(new[] { "Invalid token." });
            }

            // Validation 2: validate expiry date
            var utcExpiryDate = long.Parse(
                jwtTokenClaimsPrincipal.Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = CommonExtensions.UnixTimestampToDateTime(utcExpiryDate);

            if (expiryDate > DateTime.UtcNow)
            {
                return AuthResult.Failed(new[] { "Token has not expired yet." });
            }

            // Validation 3: validate existence of the refresh token
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Value == tokenRequest.RefreshToken);

            if (storedToken is null)
            {
                return AuthResult.Failed(new[] { "Refresh token does not exist." });
            }

            // Validation 4: validate if used
            if (storedToken.IsUsed)
            {
                return AuthResult.Failed(new[] { "Refresh token has been used." });
            }

            // Validation 5: validate if revoked
            if (storedToken.IsRevoked)
            {
                return AuthResult.Failed(new[] { "Refresh token has been revoked." });
            }

            // Validation 6: validate the id
            var jti = jwtTokenClaimsPrincipal.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedToken.JwtId != jti)
            {
                return AuthResult.Failed(new[] { "Tokens do not match." });
            }

            // Update Used Refresh Token
            storedToken.IsUsed = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            // Generate New Tokens
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
            return await GenerateJwtTokens(dbUser.Email);
        }
    }
}
