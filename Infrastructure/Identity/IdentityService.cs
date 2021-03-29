using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<bool> EmailAvailableAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is null;
        }

        public async Task<bool> CheckCredentialsAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<Result> AddToRoleAsync(string email, string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists == false) return Result.Failure(new[] { $"Role with name '{roleName}' not found." });

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Result.Failure(new[] { $"User with email '{email}' not found." });

            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (isInRole == true) return Result.Failure(new[] { $"User '{email}' is already assigned to role '{roleName}'." });

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (roleResult.Succeeded) return Result.Success();

            return Result.Failure(new[] { $"Unable to assign role '{roleName}' to user '{email}'." });
        }

        public async Task<Result> CreateRoleAsync(string roleName)
        {
            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded == false)
            {
                var errors = result.Errors.Select(x => x.Description).ToArray();
                return Result.Failure(errors);
            }

            return IdentityRoleResult.Success(role);
        }

        public async Task<Result> FindRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null) return Result.Failure(new[] { $"Unable to find role '{roleName}'" });

            return IdentityRoleResult.Success(role);
        }
    }
}
