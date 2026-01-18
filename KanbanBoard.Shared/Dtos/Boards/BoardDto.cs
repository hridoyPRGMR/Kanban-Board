namespace KanbanBoard.Shared.Dtos
{
    public class BoardDto
    {
        public Guid Id {get;init;}
        public string Name {get; init;} = default!;
        public string? Description {get;set;}
    }
}