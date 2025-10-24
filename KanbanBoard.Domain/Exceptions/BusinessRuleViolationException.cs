namespace KanbanBoard.Domain.Exception
{
    public class BusinessRuleViolationException : DomainException
    {
        public BusinessRuleViolationException(string message)
            : base(message) { }
    }
}