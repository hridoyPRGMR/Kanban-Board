using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsRevoked { get; private set; }
        public string? RevokedReason { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string UserId { get; private set; }
        public User User { get; private set; }
        public string? ReplacedByToken { get; private set; }
        public string ClientIpAddress { get; private set; }
        public string UserAgent { get; private set; }

        private RefreshToken() { }

        public RefreshToken(string token, DateTime expiryDate, string userId, string clientIpAddress, string userAgent)
        {
            Token = token;
            ExpiryDate = expiryDate;
            UserId = userId;
            ClientIpAddress = clientIpAddress;
            UserAgent = userAgent;
            IsRevoked = false;
        }

        public void Revoke(string reason, string? replacedByToken = null)
        {
            IsRevoked = true;
            RevokedReason = reason;
            RevokedAt = DateTime.UtcNow;
            ReplacedByToken = replacedByToken;
        }

        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}