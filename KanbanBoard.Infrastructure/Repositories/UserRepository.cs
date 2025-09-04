using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<User?> GetByIdAsync(Guid id) =>
            await context.Users.FindAsync(id);

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await context.Users.ToListAsync();

        public async Task<User?> GetByEmailAsync(string email) =>
            await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public void Add(User entity) => context.Users.Add(entity);

        public void Remove(User entity) => context.Users.Remove(entity);
    }
}