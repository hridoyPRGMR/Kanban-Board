using KanbanBoard.Shared.Dtos;

namespace KanbanBoard.Application.IServices
{
    public interface IBoardService : ICrudService<CreateUpdateBoardDto,BoardDto>
    {
        
    }
}