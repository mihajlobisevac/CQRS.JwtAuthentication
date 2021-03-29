using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Users.Commands.LoginUser
{
    public static class LoginUser
    {
        public record Query() : IRequest<Result>
        {
            public string Email { get; init; }
            public string Password { get; init; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IIdentityService _identityService;
            private readonly IAuthService _authService;

            public Handler(IIdentityService identityService, IAuthService authService)
            {
                _identityService = identityService;
                _authService = authService;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var validCredentials = await _identityService
                    .CheckCredentialsAsync(request.Email, request.Password);

                if (validCredentials == false) return Result.Failure(new[] { "Invalid credentials." });

                var tokenResult = await _authService.GenerateJwtTokens(request.Email);

                return tokenResult;
            }
        }
    }
}
