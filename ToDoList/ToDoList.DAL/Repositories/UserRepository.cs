using Microsoft.EntityFrameworkCore;
using ToDoListManager.App.Filters;
using ToDoListManager.App.Interfaces.Repositories;
using ToDoListManager.DAL.Entities;
using ToDoListManager.Domain.Entities;

namespace ToDoListManager.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ToDoListDbContext _context;

        public UserRepository(ToDoListDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User domainEntity)
        {
            var entityDal = new UserDal()
            {
                Id = domainEntity.Id,
                Name = domainEntity.Name,
                Email = domainEntity.Email,
                Password = domainEntity.Password,
            };

            var entityEntry = await _context.Set<UserDal>().AddAsync(entityDal);
            await _context.SaveChangesAsync();

            var createdEntity = entityEntry.Entity;

            return new User(
                createdEntity.Id, 
                createdEntity.Name, 
                createdEntity.Email, 
                createdEntity.Password);
        }

        public async Task<User> DeleteAsync(Guid id)
        {
            var entityDal = await _context.Set<UserDal>().FirstAsync(x => x.Id == id);
            var entityEntry = _context.Remove(entityDal);
            await _context.SaveChangesAsync();
            var deletedEntityDal = entityEntry.Entity;
            return MapToDomainEntity(deletedEntityDal);
        }

        public async Task<List<User>> GetAllAsync()
        {
            var entitiesDal = await _context.Set<UserDal>().ToListAsync();
            
            return entitiesDal
                .Select(entityDal => MapToDomainEntity(entityDal))
                .ToList();
        }

        public async Task<List<User>> GetByFilterAsync(UsersFilter filter)
        {
            var entitiesQuery = _context.Set<UserDal>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                entitiesQuery = entitiesQuery.Where(x => x.Name == filter.Name);

            if (!string.IsNullOrWhiteSpace(filter.Email))
                entitiesQuery = entitiesQuery.Where(x => x.Email == filter.Email);

            var filteredEntitiesDal = await entitiesQuery.ToListAsync();

            return filteredEntitiesDal
                .Select(entityDal => MapToDomainEntity(entityDal))
                .ToList();
        }

        public async Task<User> GetByIdOrThrowAsync(Guid id)
        {
            var dalEndtity = await _context.Set<UserDal>().FirstOrDefaultAsync(x => x.Id == id);
            if (dalEndtity == null)
                throw new Exception();

            return MapToDomainEntity(dalEndtity); 
        }

        public async Task<User> UpdateAsync(User domainEntity)
        {
            var dalEntity = await _context.Set<UserDal>().FirstOrDefaultAsync();
            
            if (dalEntity == null)
                throw new Exception();

            dalEntity.Name = domainEntity.Name;
            dalEntity.Email = domainEntity.Email;
            dalEntity.Password = dalEntity.Password;

            var entityEntry = _context.Update(dalEntity);
            await _context.SaveChangesAsync();
            var updatedEntity = entityEntry.Entity;
            return MapToDomainEntity(updatedEntity);
        }

        private static User MapToDomainEntity(UserDal userDal) => new(userDal.Id, userDal.Name, userDal.Email, userDal.Password);
    }
}
