using AutoMapper;
using KanbanBoard.Application.Dtos.Projects;
using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Common;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IPersistence;
using KanbanBoard.Domain.IRepositories;

namespace KanbanBoard.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ProjectService(
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ProjectDto> CreateAsync(CreateUpdateProjectDto input)
        {
            try
            {
                // Get owner id from current authenticated user
                var ownerId = _currentUserService?.UserId;
                if (ownerId == null)
                {
                    // If your design allows anonymous project creation, adjust accordingly.
                    throw new InvalidOperationException("Authenticated user required to create a project.");
                }

                var project = new Project(input.Name, input.Description, ownerId.Value);
                var savedProject = await _projectRepository.AddAsync(project);
                if (savedProject != null)
                {
                    await _unitOfWork.SaveChangesAsync();
                    return new ProjectDto(savedProject.Name, savedProject.Description, savedProject.CreatedAt);
                }
                
                throw new InvalidOperationException("Failed to create project");
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Invalid project data: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new InvalidOperationException("Project not found");
            
            await _projectRepository.RemoveAsync(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects.Select(p => new ProjectDto(p.Name, p.Description, p.CreatedAt));
        }

        public async Task<ProjectDto?> GetByidAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            return project != null ? new ProjectDto(project.Name, project.Description, project.CreatedAt) : null;
        }

        public async Task UpdateAsync(Guid id, CreateUpdateProjectDto dto)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new InvalidOperationException("Project not found");
            
            try
            {
                project.UpdateDetails(dto.Name, dto.Description);
                await _projectRepository.UpdateAsync(project);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Invalid project data: {ex.Message}", ex);
            }
        }
    }
}