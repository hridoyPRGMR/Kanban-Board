using KanbanBoard.Application.Dtos;
using KanbanBoard.Application.Dtos.Auth;
using KanbanBoard.Application.Dtos.Users;
using KanbanBoard.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace KanbanBoard.Application.IServices
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterUserDto input);
        Task<(bool Success, User? User, string? ErrorMessage)> ValidateUserCredentialsAsync(LoginDto loginDto);
        Task<User?> FindByUsernameAsync(string username);
        Task<User?> FindByIdAsync(string userId);
        Task<User?> FindByEmailAsync(string email);
        Task<IEnumerable<string>> GetUserRolesAsync(User user);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> LockUserAsync(string userId, TimeSpan lockoutDuration);
        Task<IdentityResult> UnlockUserAsync(string userId);
        Task<bool> IsUserLockedOutAsync(User user);
    }
}