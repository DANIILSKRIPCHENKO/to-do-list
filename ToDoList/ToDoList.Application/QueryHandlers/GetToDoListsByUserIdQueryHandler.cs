using MediatR;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Queries
{
    public class GetToDoListsByUserIdQueryHandler : IRequestHandler<GetToDoListsByUserIdQuery, List<ToDoList>>
    {
        private readonly IToDoListRepository _toDoListRepository;

        public GetToDoListsByUserIdQueryHandler(IToDoListRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }

        public async Task<List<ToDoList>> Handle(GetToDoListsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userToDoLists = await _toDoListRepository.GetByFilterAsync(new Filters.ToDoListFilter()
            {
                UserId = request.UserId,
            });

            return userToDoLists;
        }
    }
}
