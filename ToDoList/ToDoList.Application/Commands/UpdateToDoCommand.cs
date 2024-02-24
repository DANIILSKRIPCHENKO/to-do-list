using MediatR;
using ToDoListManager.Domain.Agregates;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Commands
{
    public class UpdateToDoCommand : IRequest<ToDoAgregate>
    {
        public Guid Id { get; set; }

        public string Handle { get; set; }

        public string Description { get; set; }

        public string Comments { get; set; }

        public ToDoStatus Status { get; set; }

        public Guid ToDoListId { get; set; }

        public Guid UserId { get; set; }
    }
}
