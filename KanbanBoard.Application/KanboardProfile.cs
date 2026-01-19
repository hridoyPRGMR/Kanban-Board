using AutoMapper;
using KanbanBoard.Application.Dtos.Projects;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Shared.Dtos;

namespace KanbanBoard.Application
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<CreateUpdateProjectDto, Project>();
            CreateMap<Project, ProjectDto>();

            CreateMap<Board,BoardDto>();
        }
    }
}