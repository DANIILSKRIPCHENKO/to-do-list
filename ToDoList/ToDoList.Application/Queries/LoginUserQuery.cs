using MediatR;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Commands
{
    public class LoginUserQuery : IRequest<User>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
