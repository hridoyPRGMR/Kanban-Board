using KanbanBoard.Application.Dtos.Projects;
using KanbanBoard.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController
    {
        public readonly IProjectService _projectService;
        
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        [HttpPost]
        public async Task<ProjectDto> CreateProjectAsync(CreateUpdateProjectDto input)
        {
            return await _projectService.CreateAsync(input);
        }
    }
    
}