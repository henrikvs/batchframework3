using Batch.Services;

namespace Batch.Orchestrator.Host;

internal sealed class SchedulerBackgroundService(IServiceScopeFactory scopeFactory, ILogger<SchedulerBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var schedulerService = scope.ServiceProvider.GetRequiredService<ISchedulerService>();
                await schedulerService.MaterializeScheduledRunRequestsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "The scheduler loop failed.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
