﻿using Application.Common.Contracts;
using Application.V1.Roles.Commands.CreateRole;
using Application.V1.Roles.Queries.GetRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Contracts;

namespace Web.Controllers.V1
{
    [Authorize(Policy = AuthConstants.Policies.Admin)]
    public class RolesController : ApiControllerBase
    {
        [HttpGet]
        [Route(ApiRoutes.GetByName)]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await Mediator.Send(new GetRoleByName.Query(name));

            return result.IsSuccessful
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRole.Command command)
        {
            var result = await Mediator.Send(command);

            return result.IsSuccessful
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
