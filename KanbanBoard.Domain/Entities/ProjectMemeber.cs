using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Entities
{
    public class ProjectMember : AuditableEntity
    {
        public Guid ProjectId {get; set;}
        public Project Project {get; set;}
        public Guid UserId {get; set;}
        public User? User {get; set;}
        public string Role {get; set;} = "Member";
    }
}