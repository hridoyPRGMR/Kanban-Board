using System;

namespace KanbanBoard.Shared.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message)
            : base(message, 400)
        {
        }
    }
}
