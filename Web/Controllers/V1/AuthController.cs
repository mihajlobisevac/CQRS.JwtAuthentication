using Application.Common.Models;
using Application.V1.Users.Commands.CreateUser;
using Application.V1.Users.Commands.LoginUser;
using Application.V1.Users.Commands.RefreshToken;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Contracts;

namespace Web.Controllers.V1
{
    public class AuthController : ApiControllerBase
    {
        [HttpPost]
        [Route(ApiRoutes.Auth.Register)]
        public async Task<IActionResult> Register([FromBody] CreateUser.Query user)
        {
            var result = await Mediator.Send(user);

            return result.IsSuccessful 
                ? Ok(result) 
                : BadRequest(result);
        }

        [HttpPost]
        [Route(ApiRoutes.Auth.Login)]
        public async Task<IActionResult> Login([FromBody] LoginUser.Query user)
        {
            var result = await Mediator.Send(user);

            return result.IsSuccessful
                ? Ok(result)
                : Unauthorized(result);
        }

        [HttpPost]
        [Route(ApiRoutes.Auth.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshJwtToken.Query tokenRequest)
        {
            var result = await Mediator.Send(tokenRequest);

            return result.IsSuccessful
                ? Ok(result)
                : Unauthorized(result);
        }
    }
}
