using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.V1.Roles.Commands.CreateRole
{
    public static class CreateRole
    {
        public record Command(string Name) : IRequest<Result>;

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _identityService.CreateRoleAsync(request.Name);
            }
        }
    }
}
