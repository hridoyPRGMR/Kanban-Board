namespace KanbanBoard.Application.IServices
{
    public interface ICrudService<TInputDto,TRetrunDto> :
        IReadService<TInputDto, TRetrunDto>,
        IWriteService<TInputDto, TRetrunDto>
        where TInputDto : class
        where TRetrunDto : class
    {
    }

}