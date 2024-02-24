using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoListManager.App.Filters;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.DAL.Entities;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.DAL.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ToDoListDbContext _context;

        public ToDoListRepository(ToDoListDbContext context)
        {
            _context = context;
        }

        public async Task<ToDoList> CreateAsync(ToDoList domainEntity)
        {
            var entityDal = new ToDoListDal()
            {
                Id = domainEntity.Id,
                Handle = domainEntity.Handle,
                Description = domainEntity.Description,
                UserId = domainEntity.UserId
            };

            var entityEntry = await _context.Set<ToDoListDal>().AddAsync(entityDal);
            await _context.SaveChangesAsync();

            var createdEntity = entityEntry.Entity;

            return new ToDoList(createdEntity.Id, createdEntity.Handle, createdEntity.Description, createdEntity.UserId);
        }

        public async Task<ToDoList> DeleteAsync(Guid id)
        {
            var entityDal = await _context.Set<ToDoListDal>().FirstAsync(x => x.Id == id);
            var entityEntry = _context.Remove(entityDal);
            await _context.SaveChangesAsync();
            var deletedEntityDal = entityEntry.Entity;
            return new ToDoList(deletedEntityDal.Id, deletedEntityDal.Handle, deletedEntityDal.Description, deletedEntityDal.UserId);
        }

        public async Task<List<ToDoList>> GetAllAsync()
        {
            var entitiesDal = await _context.Set<ToDoListDal>().ToListAsync();

            return entitiesDal
                .Select(entityDal => new ToDoList(entityDal.Id, entityDal.Handle, entityDal.Description, entityDal.UserId))
                .ToList();
        }

        public async Task<List<ToDoList>> GetByFilterAsync(ToDoListFilter filter)
        {
            var entitiesQuery = _context.Set<ToDoListDal>().AsQueryable();

            entitiesQuery = entitiesQuery.Where(x => x.UserId == filter.UserId);

            var filteredEntitiesDal = await entitiesQuery.ToListAsync();

            return filteredEntitiesDal
                .Select(x => new ToDoList(x.Id, x.Handle, x.Description, x.UserId))
                .ToList();
        }

        public async Task<ToDoList> GetByIdOrThrowAsync(Guid id)
        {
            var dalEndtity = await _context.Set<ToDoListDal>().FirstOrDefaultAsync(x => x.Id == id);
            if (dalEndtity == null)
                throw new Exception();

            return new ToDoList(dalEndtity.Id, dalEndtity.Handle, dalEndtity.Description, dalEndtity.UserId);
        }

        public async Task<ToDoList> UpdateAsync(ToDoList domainEntity)
        {
            var dalEntity = await _context.Set<ToDoListDal>().FirstOrDefaultAsync();

            if (dalEntity == null)
                throw new Exception();

            dalEntity.Handle = domainEntity.Handle;
            dalEntity.Description = domainEntity.Description;

            var entityEntry = _context.Update(dalEntity);
            await _context.SaveChangesAsync();
            var updatedEntity = entityEntry.Entity;
            return new ToDoList(
                updatedEntity.Id,
                updatedEntity.Handle,
                updatedEntity.Description,
                updatedEntity.UserId);
        }
    }
}
