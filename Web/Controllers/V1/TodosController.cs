using Application.Requests.Todos.Queries.GetTodos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Controllers.V1
{
    public class TodosController : ApiControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<TodoDto>> GetTodos()
        {
            return await Mediator.Send(new GetTodosQuery());
        }
    }
}
