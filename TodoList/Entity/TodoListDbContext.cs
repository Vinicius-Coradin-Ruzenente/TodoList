using Microsoft.EntityFrameworkCore;
using TodoList.Model;

namespace TodoList.Entity
{
    public class TodoListDbContext(DbContextOptions<TodoListDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Todos> Todos { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(e => e.Tasks);
            modelBuilder.Entity<User>().Property(t => t.CreatedAt).IsRequired();
            modelBuilder.Entity<User>().Property(t => t.UpdatedAt).IsRequired();
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "Test",
                Password = "test123",
                Email = "test@test.com",
                IsActive = true,
            });

            
            modelBuilder.Entity<Todos>().Property(t => t.CreatedAt).IsRequired();
            modelBuilder.Entity<Todos>().Property(t => t.DoneAt);
        }
    }
}
