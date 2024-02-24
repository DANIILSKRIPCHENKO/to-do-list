using MediatR;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Commands
{
    public class RegisterUserCommand : IRequest<User>
    {
        public string Name { get; set;}

        public string Email { get; set;}

        public string Password { get; set; }
    }
}
