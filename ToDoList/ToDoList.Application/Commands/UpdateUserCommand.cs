using MediatR;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Commands
{
    public class UpdateUserCommand : IRequest<User>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
