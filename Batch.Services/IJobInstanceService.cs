using Batch.Domain.Contracts;
using Batch.Domain.Entities;

namespace Batch.Services;

/// <summary>
/// Provides read access to job instances.
/// </summary>
public interface IJobInstanceService
{
    /// <summary>
    /// Gets instances for a tenant.
    /// </summary>
    Task<PagedResult<BatchJobInstance>> GetInstancesAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an instance by identifier.
    /// </summary>
    Task<BatchJobInstance> GetInstanceAsync(Guid instanceId, CancellationToken cancellationToken);
}
