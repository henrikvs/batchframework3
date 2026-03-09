using Batch.Services;

namespace Batch.Orchestrator.Host;

internal sealed class RequestFiringBackgroundService(IServiceScopeFactory scopeFactory, ILogger<RequestFiringBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var requestFiringService = scope.ServiceProvider.GetRequiredService<IRequestFiringService>();
                await requestFiringService.FireDueRunRequestsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "The request firing loop failed.");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
