namespace KanbanBoard.Domain.ValueObjects
{
    public sealed class UserName : IEquatable<UserName>
    {
        public string Value { get; }

        public UserName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Username cannot be empty", nameof(value));
            
            if (value.Length < 3)
                throw new ArgumentException("Username must be at least 3 characters long", nameof(value));
            
            if (value.Length > 50)
                throw new ArgumentException("Username cannot be longer than 50 characters", nameof(value));
            
            if (!IsValidUserName(value))
                throw new ArgumentException("Username can only contain letters, numbers, dots, hyphens, and underscores", nameof(value));
            
            Value = value;
        }

        private static bool IsValidUserName(string username)
        {
            return username.All(c => char.IsLetterOrDigit(c) || c == '.' || c == '-' || c == '_');
        }

        public bool Equals(UserName? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is UserName userName && Equals(userName);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(UserName userName) => userName.Value;
        public static explicit operator UserName(string userName) => new(userName);

        public static bool operator ==(UserName? left, UserName? right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(UserName? left, UserName? right)
        {
            return !(left == right);
        }
    }
}