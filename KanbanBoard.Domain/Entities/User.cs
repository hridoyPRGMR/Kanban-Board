using KanbanBoard.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace KanbanBoard.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; private set; }
        public ICollection<Project> Projects { get; private set; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; private set; } = [];

        private User() { }

        public User(string email, string passwordHash, string userName, string name, string phoneNumber)
        {
            Email = email;
            PasswordHash = passwordHash;
            UserName = userName;
            Name = name;
            PhoneNumber = phoneNumber;
        }
    }
}