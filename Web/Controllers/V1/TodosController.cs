using Application.Requests.Todos.Commands.CreateTodo;
using Application.Requests.Todos.Queries.GetTodo;
using Application.Requests.Todos.Queries.GetTodos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Contracts;

namespace Web.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TodosController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var result = await Mediator.Send(new GetTodos.Query());

            return result is null
                ? BadRequest()
                : Ok(result);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet]
        [Route(ApiRoutes.Todos.GetById)]
        public async Task<IActionResult> GetTodo(int id)
        {
            var result = await Mediator.Send(new GetTodo.Query(id));

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] CreateTodo.Command command)
        {
            var result = await Mediator.Send(command);

            return result is 0
                ? BadRequest()
                : Ok($"Successfully created Todo ({result})");
        }
    }
}
