namespace KanbanBoard.Domain.Services
{
    public interface IDomainValidationService
    {
        bool IsValidEmail(string email);
        bool IsValidUserName(string username);
        bool IsValidProjectName(string projectName);
        bool IsValidTaskTitle(string taskTitle);
    }
}