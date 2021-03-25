using Application.Common.Models;
using Application.Requests.Identity.Commands.RefreshToken;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> CreateUserAsync(string userName, string email, string password);
        Task<bool> EmailNotAvailableAsync(string email);
        Task<bool> EmailPasswordNotValidAsync(string email, string password);
        Task<AuthResult> GenerateJwtTokens(string email);
        Task<AuthResult> ValidateAndCreateTokensAsync(TokenRequestDto tokenRequest);
    }
}
