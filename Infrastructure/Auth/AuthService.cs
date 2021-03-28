using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Requests.Users.Commands.RefreshToken;
using Infrastructure.Identity;
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

namespace Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParams;

        public AuthService(
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

        public async Task<AuthResult> GenerateJwtTokens(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            var claimsIdentity = new ClaimsIdentity(user.GenerateClaims(roles));
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                Expires = DateTime.UtcNow.AddSeconds(1200) // should be 5-10 mins
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var createdToken = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(createdToken);
            var refreshToken = await CreateRefreshTokenAsync(user, createdToken);

            return AuthResult.Success(jwtToken, refreshToken);
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
                ExpiryDate = DateTime.UtcNow.AddMonths(5), // should be 6+ months
                Value = GeneralExtensions.GenerateRandomString(35) + Guid.NewGuid().ToString()
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
                return AuthResult.Failure(new[] { "Invalid token." });
            }

            // Validation 2: validate expiry date
            var utcExpiryDate = long.Parse(
                jwtTokenClaimsPrincipal.Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = GeneralExtensions.UnixTimestampToDateTime(utcExpiryDate);

            if (expiryDate > DateTime.UtcNow)
            {
                return AuthResult.Failure(new[] { "Token has not expired yet." });
            }

            // Validation 3: validate existence of the refresh token
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Value == tokenRequest.RefreshToken);

            if (storedToken is null)
            {
                return AuthResult.Failure(new[] { "Refresh token does not exist." });
            }

            // Validation 4: validate if used
            if (storedToken.IsUsed)
            {
                return AuthResult.Failure(new[] { "Refresh token has been used." });
            }

            // Validation 5: validate if revoked
            if (storedToken.IsRevoked)
            {
                return AuthResult.Failure(new[] { "Refresh token has been revoked." });
            }

            // Validation 6: validate the id
            var jti = jwtTokenClaimsPrincipal.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedToken.JwtId != jti)
            {
                return AuthResult.Failure(new[] { "Tokens do not match." });
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
