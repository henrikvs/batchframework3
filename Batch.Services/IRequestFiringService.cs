namespace Batch.Services;

/// <summary>
/// Turns due run requests into job instances.
/// </summary>
public interface IRequestFiringService
{
    /// <summary>
    /// Fires due planned run requests into job instances.
    /// </summary>
    Task FireDueRunRequestsAsync(CancellationToken cancellationToken);
}
