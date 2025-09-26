using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Events
{
    public class ProjectCreatedEvent : DomainEvent
    {
        public Guid ProjectId { get; }
        public string ProjectName { get; }
        public Guid OwnerId { get; }

        public ProjectCreatedEvent(Guid projectId, string projectName, Guid ownerId)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            OwnerId = ownerId;
        }
    }
}