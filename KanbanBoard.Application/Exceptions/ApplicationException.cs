using KanbanBoard.Shared.Exceptions;

namespace KanbanBoard.Application.Exception
{
    public class ApplicationException : BaseException
    {
        public ApplicationException(string message, int statusCode = 500)
            : base(message, statusCode) { }
    }
}