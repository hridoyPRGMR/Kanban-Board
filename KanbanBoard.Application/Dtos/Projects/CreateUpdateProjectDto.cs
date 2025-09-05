using System.ComponentModel.DataAnnotations;

namespace KanbanBoard.Application.Dtos.Projects
{
    public class CreateUpdateProjectDto
    {
        [Required(ErrorMessage = "Project name is required.")]
        public string Name { get; set; }

        [StringLength(500,ErrorMessage = "Description must be less than or equal 500 character.")]
        public string Description { get; set; }
    }
}