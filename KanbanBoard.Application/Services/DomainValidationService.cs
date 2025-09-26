using KanbanBoard.Domain.Services;
using System.Text.RegularExpressions;

namespace KanbanBoard.Application.Services
{
    public class DomainValidationService : IDomainValidationService
    {
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex UserNameRegex = new(
            @"^[a-zA-Z0-9._-]+$",
            RegexOptions.Compiled);

        public bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && 
                   email.Length <= 255 && 
                   EmailRegex.IsMatch(email);
        }

        public bool IsValidUserName(string username)
        {
            return !string.IsNullOrWhiteSpace(username) && 
                   username.Length >= 3 && 
                   username.Length <= 50 && 
                   UserNameRegex.IsMatch(username);
        }

        public bool IsValidProjectName(string projectName)
        {
            return !string.IsNullOrWhiteSpace(projectName) && 
                   projectName.Length <= 100;
        }

        public bool IsValidTaskTitle(string taskTitle)
        {
            return !string.IsNullOrWhiteSpace(taskTitle) && 
                   taskTitle.Length <= 200;
        }
    }
}