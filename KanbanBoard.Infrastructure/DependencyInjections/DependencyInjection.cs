using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IPersistence;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Interceptors;
using KanbanBoard.Infrastructure.Persistence;
using KanbanBoard.Infrastructure.Repositories;
using KanbanBoard.Infrastructure.Services;
using Kanboard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KanbanBoard.Infrastructure.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>((provider,options) =>
            {
                var interceptor = provider.GetRequiredService<AuditableEntitySaveChangesInterceptor>();
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(interceptor);
            });

            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Repositories
            // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectService, ProjectService>();

            return services;
        }
    }
}