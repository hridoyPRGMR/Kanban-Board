using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Domain.IRepositories
{
    public interface IBoardTaskRepository : IRepository<BoardTask>
    {
        Task<IEnumerable<BoardTask>> GetByBoardIdAsync(Guid boardId);
        Task<IEnumerable<BoardTask>> GetByAssigneeIdAsync(Guid assigneeId);
        Task<BoardTask?> GetWithCommentsAsync(Guid taskId);
    }
}