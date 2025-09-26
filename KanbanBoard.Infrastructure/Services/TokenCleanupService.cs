using KanbanBoard.Application.IServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KanbanBoard.Infrastructure.Services
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TokenCleanupService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromHours(24); // Run daily

        public TokenCleanupService(IServiceProvider serviceProvider, ILogger<TokenCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
                    
                    await tokenService.CleanupExpiredTokensAsync();
                    _logger.LogInformation("Expired tokens cleanup completed at {Time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during token cleanup at {Time}", DateTime.UtcNow);
                }

                await Task.Delay(_period, stoppingToken);
            }
        }
    }
}