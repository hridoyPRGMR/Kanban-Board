namespace KanbanBoard.Application.IServices
{
    public interface IWriteService<TInputDto,TReturnDto>
        where TInputDto : class
        where TReturnDto : class
    {
        Task<TReturnDto> CreateAsync(TInputDto input);
        Task UpdateAsync(Guid id, TInputDto dto);
        Task DeleteAsync(Guid id);
    }
    
}