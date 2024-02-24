using ToDoListManager.App.Filters;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>, IFilterableRepository<User, UsersFilter>
    {
    }
}
