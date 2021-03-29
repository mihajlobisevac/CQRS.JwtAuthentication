using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Todos.Queries.GetTodo
{
    public static class GetTodo
    {
        public record Query(int Id) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var todo = await _context.Todos
                    .FindAsync(request.Id);

                return _mapper.Map<Response>(todo);
            }
        }

        public record Response : IMapFrom<Todo>
        {
            public string Title { get; init; }
            public string Description { get; init; }
            public bool Completed { get; init; }
        }
    }
}
