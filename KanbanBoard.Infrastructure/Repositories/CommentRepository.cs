using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId)
        {
            return await _dbSet
                .Where(c => c.TaskId == taskId)
                .Include(c => c.Author)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByAuthorIdAsync(Guid authorId)
        {
            return await _dbSet
                .Where(c => c.AuthorId == authorId)
                .Include(c => c.Task)
                .ThenInclude(t => t.Board)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}