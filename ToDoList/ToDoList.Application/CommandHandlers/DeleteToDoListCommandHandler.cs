using MediatR;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Errors;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.CommandHandlers
{
    internal class DeleteToDoListCommandHandler : IRequestHandler<DeleteToDoListCommand, ToDoList>
    {
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IToDoRepository _toDoRepository;

        public DeleteToDoListCommandHandler(
            IToDoListRepository toDoListRepository,
            IToDoRepository toDoRepository)
        {
            _toDoListRepository = toDoListRepository;
            _toDoRepository = toDoRepository;
        }
        public async Task<ToDoList> Handle(DeleteToDoListCommand request, CancellationToken cancellationToken)
        {
            var entity = await _toDoListRepository.GetByIdOrThrowAsync(request.Id);
            if (entity.UserId != request.UserId)
                throw new InvalidEntityOwnerException();

            var toDos = await _toDoRepository.GetByFilterAsync(new Filters.ToDoFilter()
            {
                ToDoListId = entity.Id,
            });

            if (toDos.Any())
            {
                throw new ConflictException();
            }

            var deletedEntity = await _toDoListRepository.DeleteAsync(request.Id);
            return deletedEntity;
        }
    }
}
