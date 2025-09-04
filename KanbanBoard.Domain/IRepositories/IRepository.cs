namespace KanbanBoard.Domain.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        void Add(T entity);
        void Remove(T entity);
    }
}