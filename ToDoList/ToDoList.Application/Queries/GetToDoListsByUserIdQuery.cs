using MediatR;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Queries
{
    public class GetToDoListsByUserIdQuery : IRequest<List<ToDoList>>
    {
        public Guid UserId { get; set; }
    }
}
