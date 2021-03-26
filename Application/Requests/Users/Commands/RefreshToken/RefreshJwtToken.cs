using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Users.Commands.RefreshToken
{
    public static class RefreshJwtToken
    {
        public record Query(TokenRequestDto TokenRequest) : IRequest<AuthResult>;

        public class Handler : IRequestHandler<Query, AuthResult>
        {
            private readonly IAuthService _authService;

            public Handler(IAuthService authService)
            {
                _authService = authService;
            }

            public async Task<AuthResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _authService.ValidateAndCreateTokensAsync(request.TokenRequest);

                if (result is null)
                {
                    return AuthResult.Failure(new[] { "Failed validating tokens." });
                }

                return result;
            }
        }
    }
}
