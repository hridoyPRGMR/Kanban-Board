namespace KanbanBoard.API.Configuration
{
    public static class SecurityHeadersConfiguration
    {
        public static IServiceCollection AddSecurityHeaders(this IServiceCollection services)
        {
            // Add HSTS configuration
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            return services;
        }

        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Use HSTS in production
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            // Security headers middleware
            app.Use(async (context, next) =>
            {
                var headers = context.Response.Headers;
                
                // Prevent MIME type sniffing
                headers.Add("X-Content-Type-Options", "nosniff");
                
                // Prevent clickjacking
                headers.Add("X-Frame-Options", "DENY");
                
                // XSS protection
                headers.Add("X-XSS-Protection", "1; mode=block");
                
                // Referrer policy
                headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
                
                // Content Security Policy
                headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; font-src 'self' https:; connect-src 'self' https:;");
                
                // Remove server header
                headers.Remove("Server");
                
                // Add security headers for API
                headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                headers.Add("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
                
                await next();
            });

            return app;
        }
    }
}