using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Identity.Commands.CreateUser
{
    public static class CreateUser
    {
        public record Query(CreateUserDto User) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                if (await _identityService.EmailNotAvailableAsync(request.User.Email))
                {
                    return Response.Fail("Email not available.");
                }

                var result = await _identityService.CreateUserAsync(
                    request.User.Username,
                    request.User.Email,
                    request.User.Password);

                if (result.Succeeded)
                {
                    return Response.Success($"Your account has been successfully created, {request.User.Username}.");
                }

                return Response.Fail("Unable to create account.");
            }
        }

        public record Response
        {
            public string Description { get; private set; }
            public bool IsSuccessful { get; private set; }

            public static Response Success(string description)
            {
                return new Response 
                {
                    Description = description,
                    IsSuccessful = true
                };
            }

            public static Response Fail(string description)
            {
                return new Response
                {
                    Description = description,
                    IsSuccessful = false
                };
            }
        }
    }
}
