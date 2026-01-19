namespace KanbanBoard.Shared.Dtos
{
    public record Result(bool IsSuccess, string? ErrorMessage = null)
    {
        public static Result Success() => new(true);
        public static Result Failure(string msg) => new(false, msg);
    }

    public record Result<T>(bool IsSuccess, T? Value = default, string? ErrorMessage = null)
    {
        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(string msg) => new(false, default, msg);
    }
}
