using MediatR;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Commands
{
    public class UpdateToDoListCommand : IRequest<ToDoList>
    {
        public Guid Id { get; set; }

        public string Handle { get; set; }

        public string Description { get; set; }

        public Guid UserId { get; set; }
    }
}
