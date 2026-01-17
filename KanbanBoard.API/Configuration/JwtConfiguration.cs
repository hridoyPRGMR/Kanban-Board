using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KanbanBoard.API.Configuration
{
    public static class JwtConfiguration
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is not configured");
            var key = Encoding.UTF8.GetBytes(jwtKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = !Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Development", StringComparison.OrdinalIgnoreCase) ?? true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured"),
                    ValidAudience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured"),
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero, // Remove default 5 minute tolerance
                    RequireExpirationTime = true,
                    RequireSignedTokens = true
                };
                
                // Enhanced JWT events for better security
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        try
                        {
                            var loggerFactory = context.HttpContext.RequestServices.GetService(typeof(Microsoft.Extensions.Logging.ILoggerFactory)) as Microsoft.Extensions.Logging.ILoggerFactory;
                            var logger = loggerFactory?.CreateLogger("JwtEvents");
                            if (logger != null)
                            {
                                var sub = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "(no sub)";
                                logger.LogInformation("JWT validated for subject {Sub}", sub);
                                foreach (var c in context.Principal?.Claims ?? Array.Empty<System.Security.Claims.Claim>())
                                {
                                    logger.LogDebug("Claim {Type} = {Value}", c.Type, c.Value);
                                }
                            }
                        }
                        catch
                        {
                            // swallow logging errors to avoid breaking auth flow
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        try
                        {
                            var loggerFactory = context.HttpContext.RequestServices.GetService(typeof(Microsoft.Extensions.Logging.ILoggerFactory)) as Microsoft.Extensions.Logging.ILoggerFactory;
                            var logger = loggerFactory?.CreateLogger("JwtEvents");
                            logger?.LogWarning(context.Exception, "JWT authentication failed");

                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                context.Response.Headers["Token-Expired"] = "true";
                            }
                        }
                        catch
                        {
                            // ignore logging exceptions
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}