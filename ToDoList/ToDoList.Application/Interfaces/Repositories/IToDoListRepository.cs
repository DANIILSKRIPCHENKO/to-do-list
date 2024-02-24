using ToDoListManager.App.Filters;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Interfaces.Repositories
{
    public interface IToDoListRepository : IRepositoryBase<ToDoList>, IFilterableRepository<ToDoList, ToDoListFilter>
    {
    }
}
