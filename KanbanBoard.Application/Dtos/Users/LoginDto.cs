using System.ComponentModel.DataAnnotations;

namespace KanbanBoard.Application.Dtos.Users
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passsword required.")]
        public string Password { get; set; } = string.Empty;
    }   
}