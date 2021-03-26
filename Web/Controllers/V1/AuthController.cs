using Application.Common.Models;
using Application.Requests.Identity.Commands.CreateUser;
using Application.Requests.Identity.Commands.LoginUser;
using Application.Requests.Identity.Commands.RefreshToken;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.V1
{
    public class AuthController : ApiControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto user)
        {
            var result = await Mediator.Send(new CreateUser.Query(user));

            return result.IsSuccessful 
                ? Ok(result) 
                : BadRequest(result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto user)
        {
            var result = await Mediator.Send(new LoginUser.Query(user));

            return result.IsSuccessful
                ? Ok(result)
                : Unauthorized(result);
        }

        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenRequest)
        {
            var result = await Mediator.Send(new RefreshJwtToken.Query(tokenRequest));

            if (result is null)
            {
                return BadRequest(AuthResult.Failed(new[] { "Invalid tokens." }));
            }

            return result.IsSuccessful
                ? Ok(result)
                : Unauthorized(result);
        }
    }
}
