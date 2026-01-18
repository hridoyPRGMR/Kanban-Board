using System;

namespace KanbanBoard.Shared.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message)
            : base(message, 401)
        {
        }
    }
}
