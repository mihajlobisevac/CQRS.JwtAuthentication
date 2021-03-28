using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Todos.Queries.GetTodos
{
    public static class GetTodos
    {
        public record Query : IRequest<IEnumerable<Response>>;

        public class Handler : IRequestHandler<Query, IEnumerable<Response>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var todos = await _context.Todos
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return todos;
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
