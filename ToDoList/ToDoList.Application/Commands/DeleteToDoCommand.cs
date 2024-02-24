using MediatR;
using ToDoListManager.Domain.Agregates;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Commands
{
    public class DeleteToDoCommand : IRequest<ToDoAgregate>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
    }
}
