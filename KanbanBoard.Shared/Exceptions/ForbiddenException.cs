using System;

namespace KanbanBoard.Shared.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message)
            : base(message, 403)
        {
        }
    }
}
