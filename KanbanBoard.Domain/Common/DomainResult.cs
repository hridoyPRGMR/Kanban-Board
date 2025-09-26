namespace KanbanBoard.Domain.Common
{
    /// <summary>
    /// Domain result type to avoid Infrastructure dependencies in Domain/Application
    /// </summary>
    public class DomainResult
    {
        public bool Success { get; private set; }
        public string? ErrorMessage { get; private set; }
        public List<string> Errors { get; private set; } = new();

        protected DomainResult(bool success, string? errorMessage = null, List<string>? errors = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
            Errors = errors ?? new List<string>();
        }

        public static DomainResult SuccessResult() => new(true);
        
        public static DomainResult FailureResult(string errorMessage) => new(false, errorMessage);
        
        public static DomainResult FailureResult(List<string> errors) => new(false, errors: errors);
        
        public static DomainResult FailureResult(string errorMessage, List<string> errors) => new(false, errorMessage, errors);
    }

    /// <summary>
    /// Domain result with return value
    /// </summary>
    public class DomainResult<T> : DomainResult
    {
        public T? Value { get; private set; }

        private DomainResult(bool success, T? value = default, string? errorMessage = null, List<string>? errors = null)
            : base(success, errorMessage, errors)
        {
            Value = value;
        }

        public static DomainResult<T> SuccessResult(T value) => new(true, value);
        
        public static new DomainResult<T> FailureResult(string errorMessage) => new(false, default, errorMessage);
        
        public static new DomainResult<T> FailureResult(List<string> errors) => new(false, default, errors: errors);
    }
}