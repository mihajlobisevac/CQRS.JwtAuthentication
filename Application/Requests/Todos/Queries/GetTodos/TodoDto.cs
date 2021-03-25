using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Requests.Todos.Queries.GetTodos
{
    public class TodoDto : IMapFrom<Todo>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
