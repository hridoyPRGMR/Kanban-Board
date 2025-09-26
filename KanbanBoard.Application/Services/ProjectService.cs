using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProjectService(
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectDto> CreateAsync(CreateUpdateProjectDto input)
        {

            var project = new Project(input.Name, input.Description);
            var savedProject = await _projectRepository.AddAsync(project, autoSave: true);
            return new ProjectDto(savedProject.Name,savedProject.Description,savedProject.CreatedAt);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {

            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
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