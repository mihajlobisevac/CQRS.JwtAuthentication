using Application.Common.Models;
using Application.Requests.Users.Commands.RefreshToken;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<Result> GenerateJwtTokens(string email);
        Task<Result> ValidateAndCreateTokensAsync(TokenRequestDto tokenRequest);
    }
}
