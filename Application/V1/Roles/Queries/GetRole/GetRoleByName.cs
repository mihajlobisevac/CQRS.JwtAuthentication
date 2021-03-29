using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.V1.Roles.Queries.GetRole
{
    public static class GetRoleByName
    {
        public record Query(string Name) : IRequest<Result>;

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _identityService.FindRoleByNameAsync(request.Name);
            }
        }
    }
}
