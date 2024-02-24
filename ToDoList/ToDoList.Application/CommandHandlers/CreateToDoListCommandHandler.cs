using MediatR;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.CommandHandlers
{
    public class CreateToDoListCommandHandler : IRequestHandler<CreateToDoListCommand, ToDoList>
    {
        private readonly IToDoListRepository _toDoListRepository;

        public CreateToDoListCommandHandler(IToDoListRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }
        public async Task<ToDoList> Handle(CreateToDoListCommand request, CancellationToken cancellationToken)
        {
            var toDoListToCreate = new ToDoList(request.Handle, request.Description, request.UserId);
            var createdToDoList = await _toDoListRepository.CreateAsync(toDoListToCreate);
            return createdToDoList;
        }
    }
}
