using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Contracts;

namespace Web.Controllers.V1
{
    public class RolesController : ApiControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _roleManager.Roles;

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.GetByName)]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _roleManager.FindByNameAsync(name);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string name)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(name));

            return result.Succeeded
                ? Ok(result)
                : BadRequest();
        }
    }
}
