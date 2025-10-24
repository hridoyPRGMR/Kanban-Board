
using KanbanBoard.Shared.Exceptions;

namespace KanbanBoard.Domain.Exception
{
    public abstract class DomainException : BaseException
    {
        protected DomainException(string message)
            : base(message, 400) { }
    }
}