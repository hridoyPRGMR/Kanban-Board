using KanbanBoard.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace KanbanBoard.Infrastructure.Identity
{
    /// <summary>
    /// Infrastructure wrapper for Domain User entity to work with ASP.NET Core Identity
    /// This keeps the Domain layer pure while allowing Infrastructure to handle Identity
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        
        // Navigation properties for EF Core
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
        public virtual ICollection<Project> Projects { get; set; } = [];

        public ApplicationUser() { }

        public ApplicationUser(User domainUser)
        {
            Id = domainUser.Id.ToString();
            Email = domainUser.Email;
            UserName = domainUser.UserName;
            Name = domainUser.Name;
            PhoneNumber = domainUser.PhoneNumber;
            EmailConfirmed = domainUser.EmailConfirmed;
            AccessFailedCount = domainUser.AccessFailedCount;
            LockoutEnd = domainUser.LockoutEnd;
            LockoutEnabled = domainUser.LockoutEnabled;
        }

        public User ToDomainUser()
        {
            var user = new User(Email ?? string.Empty, PasswordHash ?? string.Empty, 
                              UserName ?? string.Empty, Name, PhoneNumber);
            
            if (EmailConfirmed) user.ConfirmEmail();
            
            for (int i = 0; i < AccessFailedCount; i++)
            {
                user.IncrementAccessFailedCount();
            }
            
            if (LockoutEnd.HasValue)
            {
                user.SetLockout(LockoutEnd);
            }

            return user;
        }
    }
}