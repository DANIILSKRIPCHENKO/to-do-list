using MediatR;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Errors;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.CommandHandlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByFilterAsync(new Filters.UsersFilter
            {
                Email = request.Email,
            });

            if (users.Any())
                throw new ConflictException();

            var user = new User(request.Name, request.Email, request.Password);
            var cretedUser = await _userRepository.CreateAsync(user);
            return cretedUser;
        }
    }
}
