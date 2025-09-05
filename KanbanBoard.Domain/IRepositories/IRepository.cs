namespace KanbanBoard.Domain.IRepositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> AddAsync(TEntity entity, bool autoSave = false);
        Task<TEntity?> UpdateAsync(TEntity entity, bool autoSave = false);
        Task RemoveAsync(TEntity entity, bool autoSave = false);
    }
}