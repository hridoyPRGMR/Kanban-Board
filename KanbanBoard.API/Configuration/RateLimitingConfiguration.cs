using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;

namespace KanbanBoard.API.Configuration
{
    public static class RateLimitingConfiguration
    {
        public static IServiceCollection AddRateLimitingConfiguration(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                // Login rate limiting - Fixed partition method
                options.AddFixedWindowLimiter("LoginPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 5; // 5 attempts
                    limiterOptions.Window = TimeSpan.FromMinutes(15); // per 15 minutes
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 2;
                });
                
                // Refresh token rate limiting
                options.AddFixedWindowLimiter("RefreshPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 10; // 10 refresh attempts
                    limiterOptions.Window = TimeSpan.FromMinutes(1); // per minute
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 2;
                });
                
                // General API rate limiting
                options.AddFixedWindowLimiter("GeneralPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 100; // 100 requests
                    limiterOptions.Window = TimeSpan.FromMinutes(1); // per minute
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 10;
                });
                
                // Global rate limiter
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 200, // 200 requests
                            Window = TimeSpan.FromMinutes(1) // per minute
                        }));
                
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429; // Too Many Requests
                    
                    var response = new
                    {
                        error = "Rate limit exceeded",
                        message = "Too many requests. Please try again later.",
                        retryAfter = GetRetryAfter(context)
                    };
                    
                    await context.HttpContext.Response.WriteAsJsonAsync(response, token);
                };
            });

            return services;
        }

        private static string GetRetryAfter(OnRejectedContext context)
        {
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                return retryAfter.TotalSeconds.ToString();
            }
            return "60"; // Default to 60 seconds
        }
    }
}