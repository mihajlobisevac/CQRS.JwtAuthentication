using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.V1.Users.Commands.CreateUser
{
    public static class CreateUser
    {
        public record Query : IRequest<Response>
        {
            public string Username { get; init; }
            public string Email { get; init; }
            public string Password { get; init; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var emailAvailable = await _identityService.EmailAvailableAsync(request.Email);

                if (emailAvailable == false)
                {
                    return Response.Fail("Email not available.");
                }

                var result = await _identityService.CreateUserAsync(
                    request.Username,
                    request.Email,
                    request.Password);

                if (result.IsSuccessful)
                {
                    return Response.Success($"Your account has been successfully created, {request.Username}.");
                }

                return Response.Fail("Unable to create account.");
            }
        }

        public record Response
        {
            public string Description { get; private set; }
            public bool IsSuccessful { get; private set; }

            public static Response Success(string description)
                => new()
                {
                    Description = description,
                    IsSuccessful = true
                };

            public static Response Fail(string description)
                => new()
                {
                    Description = description,
                    IsSuccessful = false
                };
        }
    }
}
