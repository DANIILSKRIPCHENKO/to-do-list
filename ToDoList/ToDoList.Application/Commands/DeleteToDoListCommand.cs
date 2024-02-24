using MediatR;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Commands
{
    public class DeleteToDoListCommand : IRequest<ToDoList>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
    }
}
