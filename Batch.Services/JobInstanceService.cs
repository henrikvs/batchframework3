using Batch.DataAccess;
using Batch.Domain.Contracts;
using Batch.Domain.Entities;

namespace Batch.Services;

/// <inheritdoc />
public sealed class JobInstanceService(IOrchestrationDA orchestrationDA) : IJobInstanceService
{
    /// <inheritdoc />
    public Task<PagedResult<BatchJobInstance>> GetInstancesAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) =>
        orchestrationDA.GetInstancesAsync(tenantId, page < 1 ? 1 : page, pageSize <= 0 ? 50 : pageSize, cancellationToken);

    /// <inheritdoc />
    public async Task<BatchJobInstance> GetInstanceAsync(Guid instanceId, CancellationToken cancellationToken) =>
        await orchestrationDA.GetInstanceAsync(instanceId, cancellationToken)
            ?? throw new KeyNotFoundException($"Instance {instanceId} was not found.");
}
