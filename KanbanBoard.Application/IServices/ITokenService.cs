using KanbanBoard.Domain.Entities;
using System.Security.Claims;

namespace KanbanBoard.Application.IServices
{
    public interface ITokenService
    {
        string CreateAccessToken(User user, IEnumerable<string> roles);
        RefreshToken CreateRefreshToken(Guid userId, string clientIpAddress, string userAgent);
        Task<ClaimsPrincipal?> ValidateTokenAsync(string token);
        Task<bool> ValidateRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(string token, string reason);
        Task RevokeAllUserTokensAsync(string userId, string reason);
        Task CleanupExpiredTokensAsync();
        string? GetUserIdFromToken(string token);
    }
}