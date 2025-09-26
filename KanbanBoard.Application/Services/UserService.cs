using KanbanBoard.Application.Dtos;
using KanbanBoard.Application.Dtos.Auth;
using KanbanBoard.Application.Dtos.Users;
using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IPersistence;
using KanbanBoard.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace KanbanBoard.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserDto input)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(input.Email);
                if (existingUser != null)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "DuplicateEmail",
                        Description = "Email is already registered."
                    });
                }

                existingUser = await _userManager.FindByNameAsync(input.UserName);
                if (existingUser != null)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "DuplicateUserName",
                        Description = "Username is already taken."
                    });
                }

                var user = new User(
                    input.Email,
                    string.Empty, // PasswordHash will be set by UserManager
                    input.UserName,
                    input.Name,
                    input.PhoneNumber);

                var result = await _userManager.CreateAsync(user, input.PasswordHash);
                
                if (result.Succeeded)
                {
                    // Add default role
                    await _userManager.AddToRoleAsync(user, "User");
                    _logger.LogInformation("User {Username} registered successfully", input.UserName);
                }
                else
                {
                    _logger.LogWarning("Failed to register user {Username}: {Errors}", 
                        input.UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user {Username}", input.UserName);
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "RegistrationFailed",
                    Description = "An error occurred during registration."
                });
            }
        }

        public async Task<(bool Success, User? User, string? ErrorMessage)> ValidateUserCredentialsAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.Username);
                if (user == null)
                {
                    _logger.LogWarning("Login attempt with invalid username: {Username}", loginDto.Username);
                    return (false, null, "Invalid username or password.");
                }

                // Check if user is locked out
                if (await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogWarning("Login attempt for locked out user: {Username}", loginDto.Username);
                    return (false, null, "Account is locked. Please try again later.");
                }

                var isValidPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isValidPassword)
                {
                    // Record failed login attempt
                    await _userManager.AccessFailedAsync(user);
                    _logger.LogWarning("Invalid password attempt for user: {Username}", loginDto.Username);
                    return (false, null, "Invalid username or password.");
                }

                // Reset access failed count on successful login
                await _userManager.ResetAccessFailedCountAsync(user);
                _logger.LogInformation("User {Username} logged in successfully", loginDto.Username);
                
                return (true, user, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating credentials for user {Username}", loginDto.Username);
                return (false, null, "An error occurred during login.");
            }
        }

        public async Task<User?> FindByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<User?> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Password changed successfully for user {UserId}", userId);
                }
                
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Password reset successfully for user {UserId}", userId);
                }
                
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for user {UserId}", userId);
                return false;
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> LockUserAsync(string userId, TimeSpan lockoutDuration)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "User not found."
                });
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.Add(lockoutDuration));
            if (result.Succeeded)
            {
                _logger.LogInformation("User {UserId} locked for {Duration}", userId, lockoutDuration);
            }
            
            return result;
        }

        public async Task<IdentityResult> UnlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "User not found."
                });
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                _logger.LogInformation("User {UserId} unlocked", userId);
            }
            
            return result;
        }

        public async Task<bool> IsUserLockedOutAsync(User user)
        {
            return await _userManager.IsLockedOutAsync(user);
        }
    }
}