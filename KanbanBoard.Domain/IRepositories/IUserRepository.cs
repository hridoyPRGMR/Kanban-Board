using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Domain.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}