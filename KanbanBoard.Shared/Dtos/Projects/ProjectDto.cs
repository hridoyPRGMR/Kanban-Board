namespace KanbanBoard.Application.Dtos.Projects
{
    public record ProjectDto(
        Guid Id,
        string Name,
        string? Description,
        DateTime CreatedAt);
}