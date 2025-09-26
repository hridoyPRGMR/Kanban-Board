using KanbanBoard.Application.Dtos.Auth;
using KanbanBoard.Application.Dtos.Users;
using KanbanBoard.Application.Dtos;
using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace KanbanBoard.Application.Services
{
    /// <summary>
    /// Application Service for Authentication Use Cases
    /// This orchestrates Domain and Infrastructure without exposing infrastructure details to API
    /// </summary>
    public class AuthenticationApplicationService
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthenticationApplicationService> _logger;

        public AuthenticationApplicationService(
            IIdentityService identityService,
            ITokenService tokenService,
            ILogger<AuthenticationApplicationService> logger)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto, string clientIp, string userAgent)
        {
            try
            {
                // Find user
                var user = await _identityService.FindByUsernameAsync(loginDto.Username);
                if (user == null)
                {
                    _logger.LogWarning("Login attempt with invalid username: {Username}", loginDto.Username);
                    return null;
                }

                // Check lockout
                if (await _identityService.IsLockedOutAsync(user))
                {
                    _logger.LogWarning("Login attempt for locked out user: {Username}", loginDto.Username);
                    throw new InvalidOperationException("Account is locked. Please try again later.");
                }

                // Validate password
                var isValidPassword = await _identityService.CheckPasswordAsync(user, loginDto.Password);
                if (!isValidPassword)
                {
                    await _identityService.AccessFailedAsync(user);
                    _logger.LogWarning("Invalid password attempt for user: {Username}", loginDto.Username);
                    return null;
                }

                // Reset failed attempts
                await _identityService.ResetAccessFailedCountAsync(user);
                _logger.LogInformation("User {Username} logged in successfully", loginDto.Username);

                // Get roles
                var roles = await _identityService.GetRolesAsync(user);

                // Create tokens
                var accessToken = _tokenService.CreateAccessToken(user, roles);
                var refreshToken = _tokenService.CreateRefreshToken(user.Id, clientIp, userAgent);

                return new LoginResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                    TokenType = "Bearer",
                    User = new UserInfoDto
                    {
                        Id = user.Id.ToString(),
                        Username = user.UserName,
                        Email = user.Email,
                        Name = user.Name,
                        Roles = roles
                    }
                };
            }
            catch (InvalidOperationException)
            {
                throw; // Re-throw known exceptions
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for username: {Username}", loginDto.Username);
                throw new InvalidOperationException("An error occurred during login");
            }
        }

        public async Task<bool> RegisterAsync(RegisterUserDto registerDto)
        {
            try
            {
                // Check if user exists
                var existingUser = await _identityService.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Email is already registered.");
                }

                existingUser = await _identityService.FindByUsernameAsync(registerDto.UserName);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Username is already taken.");
                }

                // Create domain user
                var user = new User(registerDto.Email, "", registerDto.UserName, registerDto.Name, registerDto.PhoneNumber);
                
                var result = await _identityService.CreateUserAsync(user, registerDto.PasswordHash);
                
                if (result)
                {
                    await _identityService.AddToRoleAsync(user, "User");
                    _logger.LogInformation("User {Username} registered successfully", registerDto.UserName);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Failed to register user {Username}", registerDto.UserName);
                    throw new InvalidOperationException("Registration failed");
                }
            }
            catch (InvalidOperationException)
            {
                throw; // Re-throw known exceptions
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user {Username}", registerDto.UserName);
                throw new InvalidOperationException("An error occurred during registration");
            }
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            return await _identityService.FindByIdAsync(userId);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var user = await _identityService.FindByIdAsync(userId);
            if (user == null) return new List<string>();
            
            return await _identityService.GetRolesAsync(user);
        }
    }
}