namespace KanbanBoard.Domain.IPersistence
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
    
}