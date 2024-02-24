using ToDoListManager.App.Filters;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Interfaces.Repositories
{
    public interface IFilterableRepository<TDomainEntity, TFilter> 
        where TDomainEntity : EntityBase
        where TFilter : FilterBase
    {
        public Task<List<TDomainEntity>> GetByFilterAsync(TFilter entity);
    }
}
