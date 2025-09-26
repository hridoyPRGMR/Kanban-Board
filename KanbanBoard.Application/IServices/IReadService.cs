namespace KanbanBoard.Application.IServices
{
    public interface IReadService<TInputDto,TReturnDto>
        where TInputDto : class
        where TReturnDto : class
    {
        Task<TReturnDto?> GetByidAsync(Guid id);
        Task<IEnumerable<TReturnDto>> GetAllAsync();
    }
}