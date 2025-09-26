using AutoMapper;
using KanbanBoard.Application.Dtos.Projects;
using KanbanBoard.Domain.Entities;

namespace KanbanBoard.API
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<CreateUpdateProjectDto, Project>();
            CreateMap<Project, ProjectDto>();
        }
    }
}