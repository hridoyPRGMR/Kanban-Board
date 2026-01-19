using System;
using KanbanBoard.Application.Dtos.Projects;
using KanbanBoard.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KanbanBoard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateUpdateProjectDto input)
        {
            var project = await _projectService.CreateAsync(input);
            // Return 201 Created with the created project DTO
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var project = await _projectService.GetByidAsync(id);
            if (project == null)
            {
                return NotFound(new { message = "Project not found" });
            }
            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] CreateUpdateProjectDto input)
        {
            var result = await _projectService.UpdateAsync(id, input);
            if (!result.IsSuccess)
            {
                var msg = result.ErrorMessage ?? string.Empty;
                if (msg.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = msg });

                return BadRequest(new { message = msg });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var result = await _projectService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                var msg = result.ErrorMessage ?? string.Empty;
                if (msg.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = msg });

                return BadRequest(new { message = msg });
            }

            return NoContent();
        }
    }
}