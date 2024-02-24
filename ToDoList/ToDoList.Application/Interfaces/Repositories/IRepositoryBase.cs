using ToDoListManager.Domain.Entities;

namespace ToDoListManager.App.Interfaces.Repositories
{
    public interface IRepositoryBase<TDomainEntity> 
        where TDomainEntity : EntityBase 
    {
        public Task<TDomainEntity> GetByIdOrThrowAsync(Guid id);

        public Task<List<TDomainEntity>> GetAllAsync();

        public Task<TDomainEntity> CreateAsync(TDomainEntity entity);

        public Task<TDomainEntity> UpdateAsync(TDomainEntity entity);

        public Task<TDomainEntity> DeleteAsync(Guid id);
    }
}
