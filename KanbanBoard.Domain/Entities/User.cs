using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities
{
    public class User : AuditableEntity
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string UserName { get; private set; }
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }

        public ICollection<Project> Projects { get; private set; } = [];

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