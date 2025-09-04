using KanbanBoard.Domain.Entities;
using KanbanBoard.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

    }

}