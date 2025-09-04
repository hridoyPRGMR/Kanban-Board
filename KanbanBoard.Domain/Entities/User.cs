using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities
{
    public class User : AuditableEntity
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        protected User() { }
    }
}