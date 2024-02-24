using Microsoft.EntityFrameworkCore;
using ToDoListManager.App.Filters;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.DAL.Entities;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.DAL.Repositories
{
    public class ToDoChangeLogRepository : IToDoChangeLogRepository
    {
        private readonly ToDoListDbContext _context;

        public ToDoChangeLogRepository(ToDoListDbContext context)
        {
            _context = context;
        }

        public async Task<ToDoChangeLog> CreateAsync(ToDoChangeLog domainEntity)
        {
            var entityDal = new ToDoChangeLogDal()
            {
                Id = domainEntity.Id,
                NewStatus = domainEntity.NewStatus,
                ToDoId = domainEntity.ToDoId,
                CreatedAt = domainEntity.CreatedAt
            };

            var entityEntry = await _context.Set<ToDoChangeLogDal>().AddAsync(entityDal);
            await _context.SaveChangesAsync();

            var createdEntity = entityEntry.Entity;

            return new ToDoChangeLog(
                createdEntity.Id, 
                createdEntity.ToDoId,  
                createdEntity.NewStatus, 
                createdEntity.CreatedAt);
        }

        public Task<ToDoChangeLog> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ToDoChangeLog>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ToDoChangeLog>> GetByFilterAsync(ToDoChangeLogFilter filter)
        {
            var entitiesQuery = _context.Set<ToDoChangeLogDal>().AsQueryable();

            entitiesQuery = entitiesQuery.Where(x => filter.ToDoIds.Contains(x.ToDoId));

            var filteredEntitiesDal = await entitiesQuery.ToListAsync();

            return filteredEntitiesDal
                .Select(x => new ToDoChangeLog(x.Id, x.ToDoId, x.NewStatus, x.CreatedAt))
                .ToList();
        }

        public Task<ToDoChangeLog> GetByIdOrThrowAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ToDoChangeLog> UpdateAsync(ToDoChangeLog entity)
        {
            throw new NotImplementedException();
        }
    }
}
