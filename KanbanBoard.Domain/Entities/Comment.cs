using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public string Content { get; private set; } = string.Empty;
        public Guid TaskId { get; private set; }
        public BoardTask Task { get; private set; } = null!;
        public Guid AuthorId { get; private set; }
        public User Author { get; private set; } = null!;

        protected Comment() { }

        public Comment(string content, Guid taskId, Guid authorId)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new KanbanBoard.Domain.Exception.DomainValidationException("Comment content cannot be empty");
            
            Content = content;
            TaskId = taskId;
            AuthorId = authorId;
        }

        public void UpdateContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new KanbanBoard.Domain.Exception.DomainValidationException("Comment content cannot be empty");
            
            Content = content;
        }
    }
}