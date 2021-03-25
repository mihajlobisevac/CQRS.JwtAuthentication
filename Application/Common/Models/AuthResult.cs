using System.Collections.Generic;

namespace Application.Common.Models
{
    public class AuthResult
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsSuccessful { get; set; }
        public ICollection<string> Errors { get; set; }

        public static AuthResult Successful(string jwtToken, string refreshToken)
            => new()
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
                IsSuccessful = true,
                Errors = null
            };

        public static AuthResult Failed(string[] errors)
            => new()
            {
                JwtToken = null,
                RefreshToken = null,
                IsSuccessful = false,
                Errors = errors
            };
    }
}
