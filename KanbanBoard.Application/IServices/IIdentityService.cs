using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Application.IServices
{
    public interface IIdentityService
    {
        Task<User?> FindByUsernameAsync(string username);
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByIdAsync(string id);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> IsLockedOutAsync(User user);
        Task AccessFailedAsync(User user);
        Task ResetAccessFailedCountAsync(User user);
        Task<bool> CreateUserAsync(User user, string password);
        Task<bool> AddToRoleAsync(User user, string role);
        Task<IEnumerable<string>> GetRolesAsync(User user);
        // Attempts a password sign-in and returns a tuple indicating success and whether the account is locked out.
        // lockoutOnFailure should be true to increment failed count and lock out according to Identity options.
        Task<(bool Succeeded, bool IsLockedOut)> PasswordSignInAsync(string username, string password, bool lockoutOnFailure);
    }
}