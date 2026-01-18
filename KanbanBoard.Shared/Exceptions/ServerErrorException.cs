using System;

namespace KanbanBoard.Shared.Exceptions
{
    public class ServerErrorException : BaseException
    {
        public ServerErrorException(string message)
            : base(message, 500)
        {
        }
    }
}
