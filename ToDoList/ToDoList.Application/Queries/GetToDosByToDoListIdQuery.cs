using MediatR;
using ToDoListManager.Domain.Agregates;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Queries
{
    public class GetToDosByToDoListIdQuery : IRequest<List<ToDoAgregate>>
    {
        public Guid ToDoListId { get; set; }
        public Guid UserId { get; set; }
    }
}
