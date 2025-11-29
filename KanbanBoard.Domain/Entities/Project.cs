using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Events;

namespace KanbanBoard.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public Guid OwnerId { get; private set; }
        public User Owner { get; private set; } = null!;
        public ICollection<Board> Boards => _boards.AsReadOnly();
        private readonly List<Board> _boards = [];
        public ICollection<ProjectMember> Members {get; set;} = [];

        protected Project() { }

        public Project(string name, string? description, Guid ownerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be empty", nameof(name));
            
            Name = name;
            Description = description;
            OwnerId = ownerId;
            
            AddDomainEvent(new ProjectCreatedEvent(Id, name, ownerId));
        }

        public void UpdateDetails(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be empty", nameof(name));
            
            Name = name;
            Description = description;
        }

        public Board AddBoard(string name, string? description)
        {
            var board = new Board(name, description, Id);
            _boards.Add(board);
            return board;
        }

        public void RemoveBoard(Guid boardId)
        {
            var board = _boards.FirstOrDefault(b => b.Id == boardId);
            if (board != null)
            {
                _boards.Remove(board);
            }
        }

        public void TransferOwnership(Guid newOwnerId)
        {
            OwnerId = newOwnerId;
        }
    }
}