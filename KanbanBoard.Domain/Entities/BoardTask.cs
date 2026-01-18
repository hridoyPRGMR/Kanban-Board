using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Events;

namespace KanbanBoard.Domain.Entities
{
    public class BoardTask : AuditableEntity
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Enums.TaskStatus Status { get; private set; }
        public Guid AssigneeId { get; private set; }
        public User Assignee { get; private set; } = null!;
        public Guid BoardId { get; private set; }
        public Board Board { get; private set; } = null!;
        public long OrderIndex { get; private set; }
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
        private readonly List<Comment> _comments = new();

        protected BoardTask() { }

        public BoardTask(string title, string description, Guid boardId, Guid assigneeId, long orderIndex)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new KanbanBoard.Domain.Exception.DomainValidationException("Task title cannot be empty");
            
            Title = title;
            Description = description ?? string.Empty;
            BoardId = boardId;
            AssigneeId = assigneeId;
            OrderIndex = orderIndex;
            Status = Enums.TaskStatus.Open;
        }

        public void UpdateDetails(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new KanbanBoard.Domain.Exception.DomainValidationException("Task title cannot be empty");
            
            Title = title;
            Description = description ?? string.Empty;
        }

        public void UpdateStatus(Enums.TaskStatus status, Guid userId)
        {
            var oldStatus = Status;
            Status = status;
            AddDomainEvent(new TaskStatusChangedEvent(Id, oldStatus, status, userId));
        }

        public void AssignTo(Guid assigneeId)
        {
            AssigneeId = assigneeId;
        }

        public void UpdateOrderIndex(long orderIndex)
        {
            OrderIndex = orderIndex;
        }

        public Comment AddComment(string content, Guid authorId)
        {
            var comment = new Comment(content, Id, authorId);
            _comments.Add(comment);
            return comment;
        }
    }
}