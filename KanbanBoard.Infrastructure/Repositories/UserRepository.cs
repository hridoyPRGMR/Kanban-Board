using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var appUser = await _userManager.FindByIdAsync(id.ToString());
            return appUser;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            return appUser;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var appUsers = await _userManager.Users.ToListAsync();
            return appUsers;
        }

        public async Task RemoveAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser != null)
            {
                await _userManager.DeleteAsync(appUser);
            }
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var appUser = await _userManager.FindByNameAsync(username);
            return appUser;
        }
    }
}