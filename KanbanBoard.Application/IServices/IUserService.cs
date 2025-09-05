using KanbanBoard.Application.Dtos;

namespace KanbanBoard.Application.IServices
{
    public interface IUserService
    {
        Task InsertUser(RegisterUserDto input);
    }
}