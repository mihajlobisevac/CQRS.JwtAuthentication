using Application.Common.Models;
using Application.Requests.Users.Commands.RefreshToken;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> CreateUserAsync(string userName, string email, string password);
        /// <summary>
        /// Looks for an existing user based on <paramref name="email"/>.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Returns true if the user is not found, otherwise false.</returns>
        Task<bool> EmailAvailableAsync(string email);
        /// <summary>
        /// Looks for an existing user based on <paramref name="email"/>, then checks if given <paramref name="password"/> matches said user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Returns true if the credentials are valid, otherwise false.</returns>
        Task<bool> CheckCredentialsAsync(string email, string password);
        Task<Result> AddToRoleAsync(string email, string roleName);
    }
}
