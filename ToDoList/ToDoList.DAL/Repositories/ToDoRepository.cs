using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoListManager.App.Filters;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.DAL.Entities;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.DAL.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ToDoListDbContext _context;

        public ToDoRepository(ToDoListDbContext context)
        {
            _context = context;
        }

        public async Task<ToDo> CreateAsync(ToDo domainEntity)
        {
            var entityDal = new ToDoDal()
            {
                Id = domainEntity.Id,
                Handle = domainEntity.Handle,
                Description = domainEntity.Description,
                Comments = domainEntity.Comments,
                ToDoListId = domainEntity.ToDoListId,
            };

            var entityEntry = await _context.Set<ToDoDal>().AddAsync(entityDal);
            await _context.SaveChangesAsync();

            var createdEntity = entityEntry.Entity;

            return MapToDomainEntity(createdEntity);
        }

        public async Task<ToDo> DeleteAsync(Guid id)
        {
            var entityDal = await _context.Set<ToDoDal>().FirstAsync(x => x.Id == id);
            var entityEntry = _context.Remove(entityDal);
            await _context.SaveChangesAsync();
            var deletedEntityDal = entityEntry.Entity;
            return MapToDomainEntity(deletedEntityDal);
        }

        public async Task<List<ToDo>> GetAllAsync()
        {
            var entitiesDal = await _context.Set<ToDoDal>().ToListAsync();

            return entitiesDal
                .Select(entityDal => MapToDomainEntity(entityDal))
                .ToList();
        }

        public async Task<List<ToDo>> GetByFilterAsync(ToDoFilter filter)
        {
            var entitiesQuery = _context.Set<ToDoDal>().AsQueryable();

            entitiesQuery = entitiesQuery.Where(x => x.ToDoListId == filter.ToDoListId);

            var filteredEntitiesDal = await entitiesQuery.ToListAsync();

            return filteredEntitiesDal
                .Select(x => MapToDomainEntity(x))
                .ToList();
        }

        public async Task<ToDo> GetByIdOrThrowAsync(Guid id)
        {
            var dalEndtity = await _context.Set<ToDoDal>().FirstOrDefaultAsync(x => x.Id == id);
            if (dalEndtity == null)
                throw new Exception();

            return MapToDomainEntity(dalEndtity);
        }

        public async Task<ToDo> UpdateAsync(ToDo domainEntity)
        {
            var dalEntity = await _context.Set<ToDoDal>().FirstOrDefaultAsync();

            if (dalEntity == null)
                throw new Exception();

            dalEntity.Handle = domainEntity.Handle;
            dalEntity.Description = domainEntity.Description;
            dalEntity.Comments = domainEntity.Comments;
            dalEntity.ToDoListId = domainEntity.ToDoListId;

            var entityEntry = _context.Update(dalEntity);
            await _context.SaveChangesAsync();
            var updatedEntity = entityEntry.Entity;
            return MapToDomainEntity(updatedEntity);
        }

        private static ToDo MapToDomainEntity(ToDoDal dalEntity) => 
            new(dalEntity.Id, dalEntity.Handle, dalEntity.Description, dalEntity.Comments, dalEntity.ToDoListId);
    }
}
