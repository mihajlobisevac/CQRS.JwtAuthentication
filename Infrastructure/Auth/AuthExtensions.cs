using Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Auth
{
    public static class AuthExtensions
    {
        public static Claim[] GenerateClaims(this ApplicationUser user)
        {
            return new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }

        public static ClaimsPrincipal GetClaimsPrincipal(this string jwtToken, TokenValidationParameters tokenParams)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var paramsWithoutValidateLifetime = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = tokenParams.ValidateIssuerSigningKey,
                IssuerSigningKey = tokenParams.IssuerSigningKey,
                ValidateIssuer = tokenParams.ValidateIssuer,
                ValidateAudience = tokenParams.ValidateAudience,
                ValidateLifetime = false,
                RequireExpirationTime = tokenParams.RequireExpirationTime,
                ClockSkew = tokenParams.ClockSkew
            };

            try
            {
                var tokenInValidationClaimsPrincipal = jwtTokenHandler.ValidateToken(
                    jwtToken,
                    paramsWithoutValidateLifetime,
                    out var validatedToken);

                if (validatedToken.IsValidSecurityAlgorithm() == false)
                {
                    return null;
                }

                return tokenInValidationClaimsPrincipal;
            }
            catch
            {
                return null;
            }
        }

        public static bool IsValidSecurityAlgorithm(this SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken)
                && jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
