using MediatR;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Errors;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.QueryHandlers
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, User>
    {
        private readonly IUserRepository _userRepository;

        public LoginUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByFilterAsync(new Filters.UsersFilter
            {
                Email = request.Email,
            });

            if (!users.Any())
                throw new EntityNotFoundException();

            if(users.Single().Password != request.Password)
                throw new ConflictException();

            return users.Single();
        }
    }
}
