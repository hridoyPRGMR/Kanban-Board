using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        void Remove(User user);
    }
}