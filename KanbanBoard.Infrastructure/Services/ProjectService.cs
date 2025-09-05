using KanbanBoard.Application.Dtos.Projects;
using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IPersistence;
using KanbanBoard.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
         private readonly IUnitOfWork _unitOfWork;

        public ProjectService(
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectDto> CreateAsync(CreateUpdateProjectDto input)
        {

            var project = new Project(input.Name, input.Description);
            _ = await _projectRepository.AddAsync(project, autoSave: true);
            return new ProjectDto(project.Name,project.Description,project.CreatedAt);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectDto>> GetAllAsync(CreateUpdateProjectDto? input)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto?> GetByidAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, CreateUpdateProjectDto dto)
        {
            throw new NotImplementedException();
        }
    }
}