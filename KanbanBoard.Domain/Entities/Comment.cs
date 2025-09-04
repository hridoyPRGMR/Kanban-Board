using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public string Content { get; private set; }
        public Guid TaskId { get; private set; }
        public Task Task { get; private set; }
        public Guid UserId { get; private set; }
        public User User{ get; private set; }
    }
}