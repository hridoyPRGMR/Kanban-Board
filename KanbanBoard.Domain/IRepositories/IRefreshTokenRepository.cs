using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Domain.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<RefreshToken?> GetByIdAsync(Guid id);
        Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(string userId);
        Task<IEnumerable<RefreshToken>> GetExpiredTokensAsync();
        Task AddAsync(RefreshToken refreshToken);
        void Remove(RefreshToken refreshToken);
        void RemoveRange(IEnumerable<RefreshToken> refreshTokens);
        Task RevokeAllUserTokensAsync(string userId, string reason);
        Task CleanupExpiredTokensAsync();
    }
}