namespace KanbanBoard.Application.Dtos.Projects
{
    public record ProjectDto(
        string Name,
        string? Description,
        DateTime CreatedAt);
}