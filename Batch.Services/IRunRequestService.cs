using Batch.Domain.Contracts;
using Batch.Domain.Entities;

namespace Batch.Services;

/// <summary>
/// Provides validated operations for manual run requests.
/// </summary>
public interface IRunRequestService
{
    /// <summary>
    /// Creates a manual run request for a tenant.
    /// </summary>
    Task<BatchRunRequest> CreateManualRunRequestAsync(int tenantId, ManualRunRequestModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Gets run requests for a tenant.
    /// </summary>
    Task<PagedResult<BatchRunRequest>> GetRunRequestsAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Cancels a planned run request.
    /// </summary>
    Task<BatchRunRequest> CancelRunRequestAsync(Guid runRequestId, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a retry run request for an existing instance.
    /// </summary>
    Task<BatchRunRequest> RetryInstanceAsync(Guid instanceId, RetryRunRequestModel model, CancellationToken cancellationToken);
}
