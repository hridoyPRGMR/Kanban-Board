using KanbanBoard.Shared.Dtos;

namespace KanbanBoard.Application.IServices
{
    public interface IWriteService<TInputDto,TReturnDto>
        where TInputDto : class
        where TReturnDto : class
    {
        Task<TReturnDto> CreateAsync(TInputDto input);
        Task<Result> UpdateAsync(Guid id, TInputDto dto);
        Task<Result> DeleteAsync(Guid id);
    }
    
}