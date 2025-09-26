using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Domain.IRepositories
{
    public interface IBoardRepository : IRepository<Board>
    {
        Task<IEnumerable<Board>> GetByProjectIdAsync(Guid projectId);
        Task<Board?> GetWithTasksAsync(Guid boardId);
    }
}