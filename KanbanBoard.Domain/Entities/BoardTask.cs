using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities
{
    public class BoardTask : AuditableEntity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TaskStatus Status { get; private set; }
        public Guid AssigneeId { get; private set; }
        public User Assignee { get; private set; }
        public Guid BoardId { get; private set; }
        public Board Board { get; private set; }
        public long OrderIndex { get; private set; }
    }
}