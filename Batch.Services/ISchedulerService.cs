namespace Batch.Services;

/// <summary>
/// Materializes near-future schedule slots into run requests.
/// </summary>
public interface ISchedulerService
{
    /// <summary>
    /// Materializes scheduled run requests inside the configured lookahead window.
    /// </summary>
    Task MaterializeScheduledRunRequestsAsync(CancellationToken cancellationToken);
}
