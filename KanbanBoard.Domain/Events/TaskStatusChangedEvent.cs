using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Events
{
    public class TaskStatusChangedEvent : DomainEvent
    {
        public Guid TaskId { get; }
        public Enums.TaskStatus OldStatus { get; }
        public Enums.TaskStatus NewStatus { get; }
        public Guid UserId { get; }

        public TaskStatusChangedEvent(Guid taskId, Enums.TaskStatus oldStatus, Enums.TaskStatus newStatus, Guid userId)
        {
            TaskId = taskId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            UserId = userId;
        }
    }
}