using MediatR;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.CommandHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.GetByIdOrThrowAsync(request.Id);

            var updatedUser = await _userRepository
                .UpdateAsync(new User(request.Id, request.Name, request.Email, request.Password));

            return updatedUser;
        }
    }
}
