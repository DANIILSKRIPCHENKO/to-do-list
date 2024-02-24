using MediatR;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Errors;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.CommandHandlers
{
    public class UpdateToDoListCommandHandler : IRequestHandler<UpdateToDoListCommand, ToDoList>
    {
        private readonly IToDoListRepository _toDoListRepository;

        public UpdateToDoListCommandHandler(IToDoListRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }

        public async Task<ToDoList> Handle(UpdateToDoListCommand request, CancellationToken cancellationToken)
        {
            var toDoList = await _toDoListRepository.GetByIdOrThrowAsync(request.Id);
            if (toDoList.UserId != request.UserId)
            {
                throw new InvalidEntityOwnerException();
            }

            var updatedToDoList = await _toDoListRepository
                .UpdateAsync(new ToDoList(request.Id, request.Handle, request.Description, request.UserId));

            return updatedToDoList;
        }
    }
}
