using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Todos.Queries.GetTodos
{
    public class GetTodosQuery : IRequest<IEnumerable<TodoDto>>
    {
    }

    public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, IEnumerable<TodoDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            var todos = await _context.Todos
                .ProjectTo<TodoDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return todos;
        }
    }
}
