namespace KanbanBoard.Domain.ValueObjects
{
    public sealed class TaskTitle : IEquatable<TaskTitle>
    {
        public string Value { get; }

        public TaskTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new KanbanBoard.Domain.Exception.DomainValidationException("Task title cannot be empty");
            
            if (value.Length > 200)
                throw new KanbanBoard.Domain.Exception.DomainValidationException("Task title cannot be longer than 200 characters");
            
            Value = value.Trim();
        }

        public bool Equals(TaskTitle? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is TaskTitle title && Equals(title);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(TaskTitle title) => title.Value;
        public static explicit operator TaskTitle(string title) => new(title);

        public static bool operator ==(TaskTitle? left, TaskTitle? right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(TaskTitle? left, TaskTitle? right)
        {
            return !(left == right);
        }
    }
}