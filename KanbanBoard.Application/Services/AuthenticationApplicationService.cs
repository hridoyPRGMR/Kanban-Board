using System;
using KanbanBoard.Application.Dtos.Auth;
using KanbanBoard.Application.Dtos.Users;
using KanbanBoard.Application.Dtos;
using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Entities;
using Microsoft.Extensions.Logging;
using KanbanBoard.Application.Exceptions;

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
                var signInResult = await _identityService.PasswordSignInAsync(loginDto.Username, loginDto.Password, lockoutOnFailure: true);

                if (signInResult.IsLockedOut)
                {
                    _logger.LogWarning("Login attempt for locked out user: {Username}", loginDto.Username);
                    throw new AccountLockedException();
                }

                if (!signInResult.Succeeded)
                    throw new InvalidCredentialsException();

                var user = await _identityService.FindByUsernameAsync(loginDto.Username)
                    ?? throw new InvalidCredentialsException();
                    
                await _identityService.ResetAccessFailedCountAsync(user);

                var roles = await _identityService.GetRolesAsync(user);
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
                        Username = user.UserName ?? string.Empty,
                        Email = user.Email ?? string.Empty,
                        Name = user.Name ?? string.Empty,
                        Roles = roles ?? Array.Empty<string>()
                    }
                };
            }
            catch (AuthenticationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for username: {Username}", loginDto.Username);
                throw new AuthenticationException("An error occurred during login");
            }
        }

        public async Task<bool> RegisterAsync(RegisterUserDto registerDto)
        {
            if (await _identityService.FindByEmailAsync(registerDto.Email) is not null)
                throw new UserAlreadyExistsException("Email is already registered.");

            if (await _identityService.FindByUsernameAsync(registerDto.UserName) is not null)
                throw new UserAlreadyExistsException("Username is already taken.");

            var user = new User(
                registerDto.Email,
                passwordHash: string.Empty,
                userName: registerDto.UserName,
                name: registerDto.Name,
                phoneNumber: registerDto.PhoneNumber
            );

            var created = await _identityService.CreateUserAsync(user, registerDto.PasswordHash);
            if (!created)
            {
                _logger.LogWarning("Failed to register user {Username}", registerDto.UserName);
                throw new RegistrationFailedException("Registration failed");
            }

            await _identityService.AddToRoleAsync(user, "User");
            return true;
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