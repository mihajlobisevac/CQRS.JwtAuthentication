using Application.Common.Models;

namespace Infrastructure.Auth
{
    public class AuthResult : Result
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }

        public static AuthResult Success(string jwtToken, string refreshToken)
            => new()
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
                IsSuccessful = true,
                Errors = null
            };
    }
}
