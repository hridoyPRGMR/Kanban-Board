using KanbanBoard.Domain.Common;

namespace KanbanBoard.Domain.Events
{
    public class TaskCreatedEvent : DomainEvent
    {
        public Guid TaskId { get; }
        public string TaskTitle { get; }
        public Guid BoardId { get; }
        public Guid AssigneeId { get; }

        public TaskCreatedEvent(Guid taskId, string taskTitle, Guid boardId, Guid assigneeId)
        {
            TaskId = taskId;
            TaskTitle = taskTitle;
            BoardId = boardId;
            AssigneeId = assigneeId;
        }
    }
}