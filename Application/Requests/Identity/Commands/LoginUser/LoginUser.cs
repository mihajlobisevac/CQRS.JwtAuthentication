using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Identity.Commands.LoginUser
{
    public static class LoginUser
    {
        public record Query(LoginUserDto User) : IRequest<AuthResult>;

        public class Handler : IRequestHandler<Query, AuthResult>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<AuthResult> Handle(Query request, CancellationToken cancellationToken)
            {
                if (await _identityService.EmailPasswordNotValidAsync(request.User.Email, request.User.Password))
                {
                    return AuthResult.Failed(new[] { "Email and/or password are incorrect." });
                }

                var tokenResult = await _identityService.GenerateJwtTokens(request.User.Email);

                return tokenResult;
            }
        }
    }
}
