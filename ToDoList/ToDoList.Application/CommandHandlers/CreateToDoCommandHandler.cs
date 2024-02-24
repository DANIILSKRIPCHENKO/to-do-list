using MediatR;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Errors;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.Domain.Agregates;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.CommandHandlers
{
    public class CreateToDoCommandHandler : IRequestHandler<CreateToDoCommand, ToDoAgregate>
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IToDoChangeLogRepository _toDoChangeLogRepository;

        public CreateToDoCommandHandler(
            IToDoRepository toDoRepository, 
            IToDoListRepository toDoListRepository,
            IToDoChangeLogRepository toDoChangeLogRepository)
        {
            _toDoRepository = toDoRepository;
            _toDoListRepository = toDoListRepository;
            _toDoChangeLogRepository = toDoChangeLogRepository;
        }
        public async Task<ToDoAgregate> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
        {
            var toDoList = await _toDoListRepository.GetByIdOrThrowAsync(request.ToDoListId);
            if (toDoList.UserId != request.UserId)
                throw new InvalidEntityOwnerException();

            //Execute in transaction
            var toDo = new ToDo(request.Handle, request.Description, request.Comments, request.ToDoListId);
            var createdToDo = await _toDoRepository.CreateAsync(toDo);

            var changeLog = new ToDoChangeLog(toDo.Id, ToDoStatus.OnHold);
            var createdChangeLog = await _toDoChangeLogRepository.CreateAsync(changeLog);

            var toDoAgregate = new ToDoAgregate(createdToDo, new List<ToDoChangeLog>() { createdChangeLog });
            return toDoAgregate;
        }
    }
}
