using Microsoft.Extensions.DependencyInjection;

namespace KanbanBoard.API.Configuration
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "https://localhost:3000") // Add HTTPS
                          .AllowAnyHeader()
                          .WithMethods("GET", "POST", "PUT", "DELETE")
                          .AllowCredentials(); // Allow credentials for JWT
                });

                // Add a more restrictive policy for production
                options.AddPolicy("Production", policy =>
                {
                    policy.WithOrigins("https://yourdomain.com") // Replace with actual domain
                          .WithHeaders("Content-Type", "Authorization")
                          .WithMethods("GET", "POST", "PUT", "DELETE")
                          .AllowCredentials();
                });

                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}