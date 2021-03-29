using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Roles.Commands.CreateRole
{
    public static class CreateRole
    {
        public record Command(string Name) : IRequest<string>;

        public class Handler : IRequestHandler<Command, string>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public Handler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var roleToAdd = new IdentityRole(request.Name);

                var result = await _roleManager.CreateAsync(roleToAdd);

                if (result.Succeeded == false) return null;

                return roleToAdd.Id;
            }
        }
    }
}
