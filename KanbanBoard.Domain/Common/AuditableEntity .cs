namespace KanbanBoard.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; private set; }
        public string? CreatedBy { get; private set; }

        public DateTime? LastModifiedAt { get; private set; }
        public string? LastModifiedBy { get; private set; }

        public bool IsArchived {get; set;}

        public void SetCreated(string userId)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = userId;
        }

        public void SetModified(string userId)
        {
            LastModifiedAt = DateTime.UtcNow;
            LastModifiedBy = userId;
        }
    }
}