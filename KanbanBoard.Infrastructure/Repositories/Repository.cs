using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

        public virtual async Task<TEntity?> AddAsync(TEntity entity, bool autoSave = false)
        {
            await _dbSet.AddAsync(entity);
            if (autoSave)
            {
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public virtual async Task<TEntity?> UpdateAsync(TEntity entity, bool autoSave = false)
        {
            _dbSet.Update(entity);
            if (autoSave)
            {
                await _context.SaveChangesAsync();
            }
            return entity;
        }

        public virtual async Task RemoveAsync(TEntity entity, bool autoSave = false)
        {
            _dbSet.Remove(entity);
            if (autoSave)
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
