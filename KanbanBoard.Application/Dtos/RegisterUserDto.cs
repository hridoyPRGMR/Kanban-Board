namespace KanbanBoard.Application.Dtos
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}