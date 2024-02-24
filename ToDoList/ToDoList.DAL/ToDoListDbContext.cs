using Microsoft.EntityFrameworkCore;
using ToDoListManager.DAL.Entities;

namespace ToDoListManager.DAL
{
    public class ToDoListDbContext : DbContext
    {
        public DbSet<UserDal> Users { get; set; }

        public DbSet<ToDoDal> ToDos { get; set; }

        public DbSet<ToDoListDal> ToDoLists { get; set; }

        public DbSet<ToDoChangeLogDal> ToDoChangeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoListDal>()
                .HasMany<ToDoDal>()
                .WithOne()
                .HasForeignKey(e => e.ToDoListId)
                .IsRequired();

            modelBuilder.Entity<UserDal>()
                .HasMany<ToDoListDal>()
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            modelBuilder.Entity<ToDoDal>()
                .HasMany<ToDoChangeLogDal>()
                .WithOne()
                .HasForeignKey(e => e.ToDoId)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=to-do-list-manager;Username=postgres;Password=660003");
        }
    }
}
