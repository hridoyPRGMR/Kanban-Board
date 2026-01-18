using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Events;
using KanbanBoard.Domain.Exception;
using System.Collections.ObjectModel;

namespace KanbanBoard.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public Guid OwnerId { get; private set; }
        public User Owner { get; private set; } = null!;

        private readonly List<Board> _boards = new();
        public IReadOnlyCollection<Board> Boards => _boards.AsReadOnly();

        private readonly List<ProjectMember> _members = new();
        public IReadOnlyCollection<ProjectMember> Members => _members.AsReadOnly();

        // Protected parameterless ctor for EF Core
        protected Project()
        {
        }

        // Private constructor used by the factory to ensure invariants are applied
        private Project(string name, string? description, Guid ownerId)
            : this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainValidationException("Project name cannot be empty");

            Name = name;
            Description = description;
            OwnerId = ownerId;

            AddDomainEvent(new ProjectCreatedEvent(Id, name, ownerId));
        }

        // Factory method for aggregate creation
        public static Project Create(string name, string? description, Guid ownerId)
        {
            return new Project(name, description, ownerId);
        }

        // Domain methods - keep invariants here
        public void UpdateDetails(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainValidationException("Project name cannot be empty");

            Name = name;
            Description = description;
        }

        public Board AddBoard(string name, string? description)
        {
            // Board constructor expects (name, projectId, description)
            var board = Board.Create(name, Id, description);
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