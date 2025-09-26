using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Identity;
using KanbanBoard.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var appUser = await _userManager.FindByIdAsync(id.ToString());
            return appUser?.ToDomainUser();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            return appUser?.ToDomainUser();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var appUsers = await _userManager.Users.ToListAsync();
            return appUsers.Select(u => u.ToDomainUser());
        }

        public async Task AddAsync(User user)
        {
            var appUser = new ApplicationUser(user);
            await _userManager.CreateAsync(appUser);
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
            return appUser?.ToDomainUser();
        }
    }
}