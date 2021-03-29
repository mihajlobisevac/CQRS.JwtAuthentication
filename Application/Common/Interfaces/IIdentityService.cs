using Application.Common.Models;
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
        /// <summary>
        /// Finds the user by <paramref name="email"/> and adds him to the role specified by <paramref name="roleName"/>.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns>A Result object which indicates whether the action has been performed successfully.</returns>
        Task<Result> AddToRoleAsync(string email, string roleName);
        /// <summary>
        /// Creates an IdentityRole passing <paramref name="roleName"/> as a constructing parameter, then adds it to database using RoleManager.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>RoleResult containing the role Id and Name if successful, otherwise Result containing the list of errors.</returns>
        Task<Result> CreateRoleAsync(string roleName);
        /// <summary>
        /// Looks up for a role based on <paramref name="roleName"/> parameter.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>RoleResult containing the role Id and Name if successful, otherwise Result containing the list of errors.</returns>
        Task<Result> FindRoleByNameAsync(string roleName);
    }
}
