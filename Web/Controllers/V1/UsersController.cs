using Application.Common.Contracts;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Contracts;

namespace Web.Controllers.V1
{
    public class UsersController : ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route(ApiRoutes.Users.AddToRoleByName)]
        public async Task<IActionResult> AddToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return NotFound($"User with email '{email}' not found.");

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists == false) return NotFound($"Role with name '{roleName}' not found.");

            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (isInRole == true) return BadRequest($"User '{email}' is already assigned to role '{roleName}'.");

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (roleResult.Succeeded) return Ok(roleResult);

            return BadRequest($"Unable to assign role '{roleName}' to user '{email}'.");
        }
    }
}
