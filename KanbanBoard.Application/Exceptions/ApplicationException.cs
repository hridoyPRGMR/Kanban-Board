using KanbanBoard.Shared.Exceptions;

namespace KanbanBoard.Application.Exceptions
{
    public class ApplicationException : BaseException
    {
        public ApplicationException(string message, int statusCode = 500)
            : base(message, statusCode) { }
    }

    public class AuthenticationException : ApplicationException
    {
        public AuthenticationException(string message)
            : base(message, 401) { }
    }

    public class InvalidCredentialsException : AuthenticationException
    {
        public InvalidCredentialsException()
            : base("Invalid username or password.") { }
    }

    public class AccountLockedException : AuthenticationException
    {
        public AccountLockedException()
            : base("Account is locked. Please try again later.") { }
    }

    public class UserAlreadyExistsException : ApplicationException
    {
        public UserAlreadyExistsException(string message)
            : base(message, 409) { }
    }

    public class RegistrationFailedException : ApplicationException
    {
        public RegistrationFailedException(string message)
            : base(message, 400) { }
    }
}