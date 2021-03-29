using Application.Common.Mappings;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Roles.Queries.GetRole
{
    public static class GetRoleByName
    {
        public record Query(string Name) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IMapper _mapper;

            public Handler(RoleManager<IdentityRole> roleManager, IMapper mapper)
            {
                _roleManager = roleManager;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.Name);

                if (role is null) return null;

                return _mapper.Map<Response>(role);
            }
        }

        public record Response : IMapFrom<IdentityRole>
        {
            public string Id { get; init; }
            public string Name { get; init; }
        }
    }
}
