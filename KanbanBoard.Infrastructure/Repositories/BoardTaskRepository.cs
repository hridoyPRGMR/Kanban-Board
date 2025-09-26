using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories
{
    public class BoardTaskRepository : Repository<BoardTask>, IBoardTaskRepository
    {
        public BoardTaskRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<BoardTask>> GetByBoardIdAsync(Guid boardId)
        {
            return await _dbSet
                .Where(t => t.BoardId == boardId)
                .Include(t => t.Assignee)
                .OrderBy(t => t.OrderIndex)
                .ToListAsync();
        }

        public async Task<IEnumerable<BoardTask>> GetByAssigneeIdAsync(Guid assigneeId)
        {
            return await _dbSet
                .Where(t => t.AssigneeId == assigneeId)
                .Include(t => t.Board)
                .ToListAsync();
        }

        public async Task<BoardTask?> GetWithCommentsAsync(Guid taskId)
        {
            return await _dbSet
                .Include(t => t.Comments)
                .ThenInclude(c => c.Author)
                .Include(t => t.Assignee)
                .Include(t => t.Board)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }
    }
}