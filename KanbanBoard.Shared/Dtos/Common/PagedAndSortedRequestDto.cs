using System.Reflection.Metadata;

namespace KanbanBoard.Shared.Dtos
{
    public record PagedAndSortedResultRequestDto{
        public int PageNumber {get; init;} = 1;
        public int PageSize {get; init;} = 10;
        public string? SortedBy {get; init;} = "Id";
        public bool IsDescending {get; init;} = false;
        public string? SearchTerm {get; init;} = null;
    };
}