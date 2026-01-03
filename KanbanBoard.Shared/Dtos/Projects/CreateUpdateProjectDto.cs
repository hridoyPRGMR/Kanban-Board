using System.ComponentModel.DataAnnotations;

namespace KanbanBoard.Application.Dtos.Projects
{
    public class CreateUpdateProjectDto
    {
        [Required(ErrorMessage = "Project name is required.")]
        [MaxLength(100, ErrorMessage = "Project name cannot be longer than 100 characters.")]
        [MinLength(1, ErrorMessage = "Project name cannot be empty.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }
    }
}