using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token) =>
            await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);

        public async Task<RefreshToken?> GetByIdAsync(Guid id) =>
            await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Id == id);

        public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(string userId) =>
            await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiryDate > DateTime.UtcNow)
                .ToListAsync();

        public async Task<IEnumerable<RefreshToken>> GetExpiredTokensAsync() =>
            await _context.RefreshTokens
                .Where(rt => rt.ExpiryDate <= DateTime.UtcNow)
                .ToListAsync();

        public async Task AddAsync(RefreshToken refreshToken) =>
            await _context.RefreshTokens.AddAsync(refreshToken);

        public void Remove(RefreshToken refreshToken) =>
            _context.RefreshTokens.Remove(refreshToken);

        public void RemoveRange(IEnumerable<RefreshToken> refreshTokens) =>
            _context.RefreshTokens.RemoveRange(refreshTokens);

        public async Task RevokeAllUserTokensAsync(string userId, string reason)
        {
            var activeTokens = await GetActiveTokensByUserIdAsync(userId);
            foreach (var token in activeTokens)
            {
                token.Revoke(reason);
            }
        }

        public async Task CleanupExpiredTokensAsync()
        {
            var expiredTokens = await GetExpiredTokensAsync();
            RemoveRange(expiredTokens);
        }
    }
}