using System.Reflection.Metadata;

namespace KanbanBoard.Shared.Dtos
{
    public record PagedAndSortedResultRequestDto(
        int PageNumber = 1,
        int PageSize = 10,
        string? SortedBy = "Id",
        bool IsDescending = false,
        string? SearchTerm = null
    );
}