using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Domain.IRepositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId);
        Task<IEnumerable<Comment>> GetByAuthorIdAsync(Guid authorId);
    }
}