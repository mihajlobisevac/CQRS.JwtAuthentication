using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.V1.Todos.Commands.CreateTodo
{
    public static class CreateTodo
    {
        public record Command : IRequest<int>
        {
            public string Title { get; init; }
            public string Description { get; init; }
            public bool Completed { get; init; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var todoToAdd = new Todo
                {
                    Title = request.Title,
                    Description = request.Description,
                    Completed = request.Completed
                };

                _context.Todos.Add(todoToAdd);

                await _context.SaveChangesAsync(cancellationToken);

                return todoToAdd.Id;
            }
        }
    }
}
