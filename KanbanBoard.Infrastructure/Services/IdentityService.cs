using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace KanbanBoard.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User?> FindByUsernameAsync(string username)
        {
            var appUser = await _userManager.FindByNameAsync(username);
            return appUser?.ToDomainUser();
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            return appUser?.ToDomainUser();
        }

        public async Task<User?> FindByIdAsync(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            return appUser?.ToDomainUser();
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;
            
            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<bool> IsLockedOutAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;
            
            return await _userManager.IsLockedOutAsync(appUser);
        }

        public async Task AccessFailedAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser != null)
            {
                await _userManager.AccessFailedAsync(appUser);
            }
        }

        public async Task ResetAccessFailedCountAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser != null)
            {
                await _userManager.ResetAccessFailedCountAsync(appUser);
            }
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            var appUser = new ApplicationUser(user);
            var result = await _userManager.CreateAsync(appUser, password);
            return result.Succeeded;
        }

        public async Task<bool> AddToRoleAsync(User user, string role)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;
            
            var result = await _userManager.AddToRoleAsync(appUser, role);
            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetRolesAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return new List<string>();
            
            return await _userManager.GetRolesAsync(appUser);
        }
    }
}