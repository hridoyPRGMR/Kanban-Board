using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.IPersistence;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Infrastructure.Interceptors;
using KanbanBoard.Infrastructure.Persistence;
using KanbanBoard.Infrastructure.Repositories;
using KanbanBoard.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KanbanBoard.Infrastructure.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {   
            // Database Context
            services.AddDbContext<AppDbContext>((provider,options) =>
            {
                var interceptor = provider.GetRequiredService<AuditableEntitySaveChangesInterceptor>();
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(interceptor);
            });

            // Infrastructure Services (implementations)
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityService, IdentityService>();

            // Repositories (Infrastructure implementations)
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<IBoardTaskRepository, BoardTaskRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // Background services
            services.AddHostedService<TokenCleanupService>();

            return services;
        }
    }
}