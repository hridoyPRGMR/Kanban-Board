using System.ComponentModel.DataAnnotations;

namespace KanbanBoard.Shared.Dtos
{
    public class CreateUpdateBoardDto
    {

        [Required,StringLength(256,MinimumLength = 3)]
        public string Name {get; init;} = default!;
        [StringLength(1024)]
        public string? Description {get; init;}
        [Required]
        public Guid ProjectId {get; init;}
    }
}