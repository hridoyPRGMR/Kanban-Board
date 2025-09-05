using KanbanBoard.Application.Dtos.Projects;
using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Application.IServices
{
    public interface IProjectService : ICrudService<CreateUpdateProjectDto,ProjectDto>
    {
        
    }
}