using System.Text.RegularExpressions;

namespace KanbanBoard.Domain.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new KanbanBoard.Domain.Exception.DomainValidationException("Email cannot be empty");
            
            if (!EmailRegex.IsMatch(value))
                throw new KanbanBoard.Domain.Exception.DomainValidationException("Invalid email format");
            
            Value = value.ToLowerInvariant();
        }

        public bool Equals(Email? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is Email email && Equals(email);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(Email email) => email.Value;
        public static explicit operator Email(string email) => new(email);

        public static bool operator ==(Email? left, Email? right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(Email? left, Email? right)
        {
            return !(left == right);
        }
    }
}