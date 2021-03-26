using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
    }
}
