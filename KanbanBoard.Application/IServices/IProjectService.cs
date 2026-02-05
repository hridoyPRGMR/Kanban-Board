using KanbanBoard.Application.Dtos;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Shared.Dtos;

namespace KanbanBoard.Application.IServices
{
    public interface IProjectService : ICrudService<CreateUpdateProjectDto,ProjectDto>
    {
        Task<PagedResponseDto<ProjectDto>> GetPaginatedProjects(ProjectFilterRequestDto input);
    }
}