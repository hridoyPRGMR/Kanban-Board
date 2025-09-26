using KanbanBoard.Application.Configuration;
using KanbanBoard.Application.IServices;
using KanbanBoard.Application.Services;
using KanbanBoard.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KanbanBoard.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuration
            var jwtOptions = new JwtOptions();
            configuration.GetSection("Jwt").Bind(jwtOptions);
            services.AddSingleton(jwtOptions);

            // Application Services (Use Case orchestration)
            services.AddScoped<AuthenticationApplicationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IDomainValidationService, DomainValidationService>();

            // AutoMapper
            services.AddAutoMapper(typeof(ApplicationDependencyInjection).Assembly);

            return services;
        }
    }
}