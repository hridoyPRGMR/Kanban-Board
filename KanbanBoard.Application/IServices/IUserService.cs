using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Application.IServices
{
    /// <summary>
    /// Simple domain-focused user service interface
    /// Infrastructure will implement this without exposing Identity details
    /// </summary>
    public interface IUserService
    {
        Task<User?> FindByIdAsync(string userId);
        Task<User?> FindByUsernameAsync(string username);
        Task<User?> FindByEmailAsync(string email);
    }
}