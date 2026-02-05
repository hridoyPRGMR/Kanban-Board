using AutoMapper;
using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Common;
using KanbanBoard.Shared.Exceptions;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IPersistence;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Shared.Dtos;
using KanbanBoard.Application.Dtos;

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
            // Get owner id from current authenticated user
            var ownerId = _currentUserService?.UserId;
            if (ownerId == null)
            {
                // If your design allows anonymous project creation, adjust accordingly.
                throw new UnauthorizedException("Authenticated user required to create a project.");
            }

            var project = Project.Create(input.Name, input.Description, ownerId.Value);
            var savedProject = await _projectRepository.AddAsync(project);
            // Persist changes centrally via UnitOfWork so transaction boundaries and
            // post-commit domain event dispatching (if implemented) work correctly.
            await _unitOfWork.SaveChangesAsync();

            if (savedProject != null)
            {
                return _mapper.Map<ProjectDto>(savedProject);
            }

            throw new ServerErrorException("Failed to create project");
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return Result.Failure("Project not found");

            await _projectRepository.RemoveAsync(project);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects.Select(p => new ProjectDto(p.Id, p.Name, p.Description, p.CreatedAt));
        }

        public async Task<ProjectDto?> GetByidAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            return project != null ? new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt) : null;
        }

        public async Task<Result> UpdateAsync(Guid id, CreateUpdateProjectDto dto)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return Result.Failure("Project not found");

            project.UpdateDetails(dto.Name, dto.Description);
            await _projectRepository.UpdateAsync(project);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<PagedResponseDto<ProjectDto>> GetPaginatedProjects(ProjectFilterRequestDto input)
        {
            var projects = await _projectRepository.GetPagedAsync(_currentUserService.GetRequiredUserId(),input);
            
            return projects;
        }
    }
}