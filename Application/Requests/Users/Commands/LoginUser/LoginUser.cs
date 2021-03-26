using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Users.Commands.LoginUser
{
    public static class LoginUser
    {
        public record Query(LoginUserDto User) : IRequest<AuthResult>;

        public class Handler : IRequestHandler<Query, AuthResult>
        {
            private readonly IIdentityService _identityService;
            private readonly IAuthService _authService;

            public Handler(IIdentityService identityService, IAuthService authService)
            {
                _identityService = identityService;
                _authService = authService;
            }

            public async Task<AuthResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var validCredentials = await _identityService
                    .CheckCredentialsAsync(request.User.Email, request.User.Password);

                if (validCredentials == false)
                {
                    return AuthResult.Failure(new[] { "Invalid credentials." });
                }

                var tokenResult = await _authService.GenerateJwtTokens(request.User.Email);

                return tokenResult;
            }
        }
    }
}
