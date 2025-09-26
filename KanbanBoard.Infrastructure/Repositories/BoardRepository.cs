using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories
{
    public class BoardRepository : Repository<Board>, IBoardRepository
    {
        public BoardRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Board>> GetByProjectIdAsync(Guid projectId)
        {
            return await _dbSet
                .Where(b => b.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<Board?> GetWithTasksAsync(Guid boardId)
        {
            return await _dbSet
                .Include(b => b.Tasks)
                .ThenInclude(t => t.Assignee)
                .FirstOrDefaultAsync(b => b.Id == boardId);
        }
    }
}