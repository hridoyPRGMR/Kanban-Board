using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; private set; }
        public string OwnerId { get; private set; }
        public User Owner { get; private set; }

        protected Project() { }
    }
}