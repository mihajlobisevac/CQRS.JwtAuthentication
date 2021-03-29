using Application.Common.Models;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<Result> GenerateJwtTokens(string email);
        Task<Result> ValidateAndCreateTokensAsync(string jwtToken, string refreshToken);
    }
}
