using KanbanBoard.Application.Dtos;
using KanbanBoard.Application.Dtos.Auth;
using KanbanBoard.Application.Dtos.Users;
using KanbanBoard.Application.IServices;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Domain.IPersistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace KanbanBoard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationApplicationService _authService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            AuthenticationApplicationService authService,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost("login")]
        [EnableRateLimiting("LoginPolicy")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var clientIpAddress = GetClientIpAddress();
                var userAgent = Request.Headers["User-Agent"].ToString();
                
                var loginResponse = await _authService.LoginAsync(model, clientIpAddress, userAgent);
                
                if (loginResponse == null)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                // Save refresh token to database
                var refreshTokenEntity = _tokenService.CreateRefreshToken(
                    Guid.Parse(loginResponse.User.Id), 
                    clientIpAddress, 
                    userAgent);
                    
                await _refreshTokenRepository.AddAsync(refreshTokenEntity);
                await _unitOfWork.SaveChangesAsync();

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for username: {Username}", model.Username);
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        [HttpPost("register")]
        [EnableRateLimiting("RegisterPolicy")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var success = await _authService.RegisterAsync(model);
                
                if (success)
                {
                    return Ok(new { message = "User registered successfully" });
                }
                
                return BadRequest(new { message = "Registration failed" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for username: {Username}", model.UserName);
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }

        [HttpPost("refresh")]
        [EnableRateLimiting("RefreshPolicy")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validate refresh token
                var isValidRefreshToken = await _tokenService.ValidateRefreshTokenAsync(model.RefreshToken);
                if (!isValidRefreshToken)
                {
                    _logger.LogWarning("Invalid refresh token used");
                    return Unauthorized(new { message = "Invalid refresh token" });
                }

                // Get refresh token from database
                var refreshToken = await _refreshTokenRepository.GetByTokenAsync(model.RefreshToken);
                if (refreshToken == null || !refreshToken.IsActive)
                {
                    _logger.LogWarning("Refresh token not found or inactive");
                    return Unauthorized(new { message = "Invalid refresh token" });
                }

                // Get user from auth service
                var user = await _authService.GetUserByIdAsync(refreshToken.UserId.ToString());
                if (user == null)
                {
                    _logger.LogWarning("User not found for refresh token");
                    return Unauthorized(new { message = "Invalid refresh token" });
                }

                // Get user roles
                var roles = await _authService.GetUserRolesAsync(refreshToken.UserId.ToString());

                // Create new access token
                var newAccessToken = _tokenService.CreateAccessToken(user, roles);
                
                // Get client information
                var clientIpAddress = GetClientIpAddress();
                var userAgent = Request.Headers["User-Agent"].ToString();
                
                // Create new refresh token
                var newRefreshToken = _tokenService.CreateRefreshToken(user.Id, clientIpAddress, userAgent);
                
                // Revoke old refresh token
                refreshToken.Revoke("Replaced by new token", newRefreshToken.Token);
                
                // Save new refresh token
                await _refreshTokenRepository.AddAsync(newRefreshToken);
                await _unitOfWork.SaveChangesAsync();

                var response = new TokenResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken.Token,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(30), // From config
                    TokenType = "Bearer"
                };

                _logger.LogInformation("Token refreshed for user {UserId}", user.Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, new { message = "An error occurred during token refresh" });
            }
        }

        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenDto model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                await _tokenService.RevokeRefreshTokenAsync(model.RefreshToken, "Revoked by user");
                
                _logger.LogInformation("Refresh token revoked for user {UserId}", userId);
                return Ok(new { message = "Token revoked successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking token");
                return StatusCode(500, new { message = "An error occurred while revoking token" });
            }
        }

        [HttpPost("revoke-all")]
        [Authorize]
        public async Task<IActionResult> RevokeAllTokens()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                await _tokenService.RevokeAllUserTokensAsync(userId, "All tokens revoked by user");
                
                _logger.LogInformation("All refresh tokens revoked for user {UserId}", userId);
                return Ok(new { message = "All tokens revoked successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking all tokens");
                return StatusCode(500, new { message = "An error occurred while revoking tokens" });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                if (!string.IsNullOrEmpty(model.RefreshToken))
                {
                    await _tokenService.RevokeRefreshTokenAsync(model.RefreshToken, "User logout");
                }
                
                _logger.LogInformation("User {UserId} logged out", userId);
                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, new { message = "An error occurred during logout" });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var user = await _authService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var roles = await _authService.GetUserRolesAsync(userId);
                
                var userInfo = new UserInfoDto
                {
                    Id = user.Id.ToString(),
                    Username = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Name = user.Name,
                    Roles = roles
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user");
                return StatusCode(500, new { message = "An error occurred while getting user information" });
            }
        }

        private string GetClientIpAddress()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }
            return ipAddress;
        }
    }
}