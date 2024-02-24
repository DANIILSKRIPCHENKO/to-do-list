using MediatR;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Commands
{
    public class CreateToDoListCommand : IRequest<ToDoList>
    {
        public string Handle { get; set; }

        public string Description { get; set; }

        public Guid UserId { get; set; }
    }
}
