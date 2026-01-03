namespace KanbanBoard.Shared.Dtos
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "")
            => new() { Success = true, Data = data, Message = message };

        public static ApiResponse<T> FailureResponse(string message)
            => new() { Success = false, Message = message };
    }
}