using Application.Common.Models;

namespace Infrastructure.Identity
{
    public class RoleResult : Result
    {
        public string RoleId { get; set; }

        public static RoleResult Success(string id)
            => new()
            {
                RoleId = id,
                IsSuccessful = true,
                Errors = null
            };
    }
}
