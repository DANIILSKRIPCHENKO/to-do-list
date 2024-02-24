using MediatR;
using ToDoListManager.App.Errors;
using ToDoListManager.App.Filters;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.App.Queries;
using ToDoListManager.Domain.Agregates;

namespace ToDoListManager.App.QueryHandlers
{
    internal class GetToDosByToDoListIdQueryHandler : IRequestHandler<GetToDosByToDoListIdQuery, List<ToDoAgregate>>
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IToDoChangeLogRepository _toDoChangeLogRepository;

        public GetToDosByToDoListIdQueryHandler(
            IToDoRepository toDoRepository, 
            IToDoListRepository toDoListRepository, 
            IToDoChangeLogRepository toDoChangeLogRepository)
        {
            _toDoRepository = toDoRepository;
            _toDoListRepository = toDoListRepository;
            _toDoChangeLogRepository = toDoChangeLogRepository;
        }

        public async Task<List<ToDoAgregate>> Handle(GetToDosByToDoListIdQuery request, CancellationToken cancellationToken)
        {
            var toDoList = await _toDoListRepository.GetByIdOrThrowAsync(request.ToDoListId);
            if (toDoList.UserId != request.UserId)
                throw new InvalidEntityOwnerException();

            var toDoFilter = new ToDoFilter()
            {
                ToDoListId = request.ToDoListId,
            };

            var filteredEntities = await _toDoRepository.GetByFilterAsync(toDoFilter);
            var changeLogs = await _toDoChangeLogRepository.GetByFilterAsync(new ToDoChangeLogFilter()
            {
                ToDoIds = filteredEntities.Select(x => x.Id).ToList()
            });

            return filteredEntities
                .Select(todo => new ToDoAgregate(
                    todo, 
                    changeLogs
                    .Where(cl => cl.ToDoId == todo.Id)
                    .ToList()))
                .ToList();
        }
    }
}
