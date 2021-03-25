using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Identity.Commands.RefreshToken
{
    public static class RefreshJwtToken
    {
        public record Query(TokenRequestDto TokenRequest) : IRequest<AuthResult>;

        public class Handler : IRequestHandler<Query, AuthResult>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<AuthResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _identityService
                    .ValidateAndCreateTokensAsync(request.TokenRequest);

                if (result is null)
                {
                    return AuthResult.Failed(new[] { "Failed validating tokens." });
                }

                return result;
            }
        }
    }
}
