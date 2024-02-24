using ToDoListManager.App.Filters;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Interfaces.Repositories
{
    public interface IToDoChangeLogRepository : IRepositoryBase<ToDoChangeLog>, IFilterableRepository<ToDoChangeLog, ToDoChangeLogFilter>
    {
    }
}
