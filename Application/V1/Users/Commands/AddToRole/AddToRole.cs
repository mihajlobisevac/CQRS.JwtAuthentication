using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.V1.Users.Commands.AddToRole
{
    public static class AddToRole
    {
        public record Command(string Email, string RoleName) : IRequest<Result>;

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _identityService.AddToRoleAsync(request.Email, request.RoleName);
            }
        }
    }
}
