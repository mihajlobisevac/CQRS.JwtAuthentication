using Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class RoleResult : Result
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public static RoleResult Success(IdentityRole role)
            => new()
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsSuccessful = true,
                Errors = null
            };
    }
}
