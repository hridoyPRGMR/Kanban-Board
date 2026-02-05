using KanbanBoard.Application.Dtos;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Persistence;
using KanbanBoard.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context) { }

        public async Task<bool> ExistById(Guid id)
        {
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }

        public async Task<PagedResponseDto<ProjectDto>> GetPagedAsync(Guid userId, ProjectFilterRequestDto input)
        {
            var query = _context.Projects
                .AsNoTracking()
                .Where(p => p.OwnerId == userId
                         || p.Members.Any(x => x.UserId == userId));

            if (!string.IsNullOrWhiteSpace(input.SearchTerm))
            {
                var pattern = $"%{input.SearchTerm.Trim()}%";
                query = query.Where(p =>
                    EF.Functions.ILike(p.Name, pattern)
                );
            }

            query = input.SortedBy?.ToLower() switch
            {
                "name" => input.IsDescending
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),

                _ => input.IsDescending
                    ? query.OrderByDescending(p => p.CreatedAt)
                    : query.OrderBy(p => p.CreatedAt)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .Select(p => new ProjectDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CreatedAt
                ))
                .ToListAsync();

            return new PagedResponseDto<ProjectDto>(
                items,
                totalCount,
                input.PageNumber,
                input.PageSize
            );
        }

    }
}