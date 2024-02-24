using System.Net.Http.Headers;
using MediatR;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Errors;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.Domain.Agregates;

namespace ToDoListManager.App.CommandHandlers
{
    public class DeleteToDoCommandHandler : IRequestHandler<DeleteToDoCommand, ToDoAgregate>
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IToDoChangeLogRepository _toDoChangeLogRepository;

        public DeleteToDoCommandHandler(
            IToDoRepository toDoRepository, 
            IToDoListRepository toDoListRepository,
            IToDoChangeLogRepository toDoChangeLogRepository)
        {
            _toDoRepository = toDoRepository;
            _toDoListRepository = toDoListRepository;
            _toDoChangeLogRepository = toDoChangeLogRepository;
        }
        
        public async Task<ToDoAgregate> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _toDoRepository.GetByIdOrThrowAsync(request.Id);
            var toDoList = await _toDoListRepository.GetByIdOrThrowAsync(todo.ToDoListId);
            var chengeLogs = await _toDoChangeLogRepository.GetByFilterAsync(new Filters.ToDoChangeLogFilter()
            {
                ToDoIds = new List<Guid> { todo.Id }
            });
            
            if (toDoList.UserId != request.UserId)
                throw new InvalidEntityOwnerException();

            var deletedToDo = await _toDoRepository.DeleteAsync(request.Id);

            return new ToDoAgregate(deletedToDo, chengeLogs);
        }
    }
}
