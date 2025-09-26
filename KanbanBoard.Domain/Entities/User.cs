using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities
{
    public class User : AuditableEntity
    {
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string UserName { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string? PhoneNumber { get; private set; }
        public bool EmailConfirmed { get; private set; }
        public int AccessFailedCount { get; private set; }
        public DateTimeOffset? LockoutEnd { get; private set; }
        public bool LockoutEnabled { get; private set; }
        public ICollection<Project> Projects { get; private set; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; private set; } = [];

        private User() { }

        public User(string email, string passwordHash, string userName, string name, string? phoneNumber = null)
        {
            Email = email;
            PasswordHash = passwordHash;
            UserName = userName;
            Name = name;
            PhoneNumber = phoneNumber;
            EmailConfirmed = false;
            AccessFailedCount = 0;
            LockoutEnabled = true;
        }

        public void UpdatePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
        }

        public void IncrementAccessFailedCount()
        {
            AccessFailedCount++;
        }

        public void ResetAccessFailedCount()
        {
            AccessFailedCount = 0;
        }

        public void SetLockout(DateTimeOffset? lockoutEnd)
        {
            LockoutEnd = lockoutEnd;
        }
    }
}