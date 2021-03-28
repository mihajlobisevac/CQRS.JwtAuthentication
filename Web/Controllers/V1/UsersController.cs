using Application.Common.Contracts;
using Application.Requests.Users.Commands.AddToRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Contracts;

namespace Web.Controllers.V1
{
    [Authorize(Policy = AuthConstants.Policies.Admin)]
    public class UsersController : ApiControllerBase
    {
        [HttpPost]
        [Route(ApiRoutes.Users.AddToRoleByName)]
        public async Task<IActionResult> AddToRole([FromBody] AddToRole.Command command)
        {
            var result = await Mediator.Send(command);

            return result.Succeeded == true
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
