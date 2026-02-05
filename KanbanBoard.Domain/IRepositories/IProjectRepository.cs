using KanbanBoard.Application.Dtos;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Shared.Dtos;

namespace KanbanBoard.Domain.IRepositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<bool> ExistById (Guid id);
        Task<PagedResponseDto<ProjectDto>> GetPagedAsync(Guid userId, ProjectFilterRequestDto input);
    }
}