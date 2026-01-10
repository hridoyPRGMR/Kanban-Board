using KanbanBoard.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace KanbanBoard.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; private set; } = string.Empty;
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

        public User(Guid id, string email, string passwordHash, string userName, string name, string? phoneNumber = null)
            : this(email, passwordHash, userName, name, phoneNumber)
        {
            Id = id;
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