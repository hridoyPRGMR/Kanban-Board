using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Events;

namespace KanbanBoard.Domain.Entities
{
    public class Board : AuditableEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; } = null!;
        public IReadOnlyCollection<BoardTask> Tasks => _tasks.AsReadOnly();
        private readonly List<BoardTask> _tasks = [];

        protected Board() { }

        public Board(string name, string? description, Guid projectId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Board name cannot be empty", nameof(name));
            
            Name = name;
            Description = description;
            ProjectId = projectId;
        }

        public void UpdateDetails(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Board name cannot be empty", nameof(name));
            
            Name = name;
            Description = description;
        }

        public BoardTask AddTask(string title, string description, Guid assigneeId)
        {
            var orderIndex = _tasks.Count > 0 ? _tasks.Max(t => t.OrderIndex) + 1 : 1;
            var task = new BoardTask(title, description, Id, assigneeId, orderIndex);
            _tasks.Add(task);
            AddDomainEvent(new TaskCreatedEvent(task.Id, title, Id, assigneeId));
            return task;
        }

        public void RemoveTask(Guid taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                _tasks.Remove(task);
            }
        }
    }
}