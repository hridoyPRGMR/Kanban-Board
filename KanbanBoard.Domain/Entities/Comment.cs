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
                throw new ArgumentException("Comment content cannot be empty", nameof(content));
            
            Content = content;
            TaskId = taskId;
            AuthorId = authorId;
        }

        public void UpdateContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Comment content cannot be empty", nameof(content));
            
            Content = content;
        }
    }
}