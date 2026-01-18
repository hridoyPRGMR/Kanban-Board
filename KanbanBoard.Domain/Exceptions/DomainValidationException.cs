namespace KanbanBoard.Domain.Exception
{
    public class DomainValidationException : DomainException
    {
        public DomainValidationException(string message)
            : base(message)
        {
        }
    }
}
