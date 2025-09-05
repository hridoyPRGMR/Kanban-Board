using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Persistence;
using KanbanBoard.Infrastructure.Repositories;

namespace Kanboard.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>,IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context) { }
    }
}