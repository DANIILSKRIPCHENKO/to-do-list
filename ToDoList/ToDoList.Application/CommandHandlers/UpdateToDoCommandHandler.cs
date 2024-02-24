using MediatR;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Errors;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.Domain.Agregates;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.CommandHandlers
{
    internal class UpdateToDoCommandHandler : IRequestHandler<UpdateToDoCommand, ToDoAgregate>
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IToDoChangeLogRepository _toDoChangeLogRepository;

        public UpdateToDoCommandHandler(
            IToDoRepository toDoRepository, 
            IToDoListRepository toDoListRepository,
            IToDoChangeLogRepository toDoChangeLogRepository)
        {
            _toDoRepository = toDoRepository;
            _toDoListRepository = toDoListRepository;
            _toDoChangeLogRepository = toDoChangeLogRepository;
        }

        public async Task<ToDoAgregate> Handle(UpdateToDoCommand request, CancellationToken cancellationToken)
        {
            var oldToDo = await _toDoRepository.GetByIdOrThrowAsync(request.Id);
            var oldToDoList = await _toDoListRepository.GetByIdOrThrowAsync(oldToDo.ToDoListId);
            var oldChenageLogs = await _toDoChangeLogRepository.GetByFilterAsync(new Filters.ToDoChangeLogFilter
            {
                ToDoIds = new List<Guid> { oldToDo.Id },
            });

            if (oldToDoList.UserId != request.UserId)
                throw new InvalidEntityOwnerException();

            var newToDoList = await _toDoListRepository.GetByIdOrThrowAsync(request.ToDoListId);
            if(newToDoList.UserId != request.UserId)
                throw new InvalidEntityOwnerException();

            var toDo = new ToDo(request.Id, request.Comments, request.Handle, request.Description, request.ToDoListId);
            var updatedToDo = await _toDoRepository.UpdateAsync(toDo);

            if(oldChenageLogs.Last().NewStatus != request.Status)
            {
                var changeLogToCreate = new ToDoChangeLog(updatedToDo.Id, request.Status);
                var createdChengeLog = await _toDoChangeLogRepository.CreateAsync(changeLogToCreate);
                return new ToDoAgregate(updatedToDo, new List<ToDoChangeLog>() { createdChengeLog });
            }

            return new ToDoAgregate(updatedToDo, oldChenageLogs);
        }
    }
}
