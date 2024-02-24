using ToDoListManager.App.Filters;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Interfaces.Repositories
{
    public interface IToDoRepository : IRepositoryBase<ToDo>, IFilterableRepository<ToDo, ToDoFilter>
    {

    }
}
