using Batch.Domain.Contracts;
using Batch.Domain.Entities;

namespace Batch.DataAccess;

/// <summary>
/// Provides data access operations for orchestration state such as run requests and instances.
/// </summary>
public interface IOrchestrationDA
{
    /// <summary>
    /// Adds a manual or API-created run request.
    /// </summary>
    Task AddRunRequestAsync(BatchRunRequest runRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Attempts to add a schedule-created run request and returns whether it was inserted.
    /// </summary>
    Task<bool> TryAddScheduledRunRequestAsync(BatchRunRequest runRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a run request by identifier.
    /// </summary>
    Task<BatchRunRequest?> GetRunRequestAsync(Guid runRequestId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paged list of run requests for a tenant.
    /// </summary>
    Task<PagedResult<BatchRunRequest>> GetRunRequestsAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all enabled schedules.
    /// </summary>
    Task<IReadOnlyList<BatchSchedule>> GetEnabledSchedulesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets due planned run requests.
    /// </summary>
    Task<IReadOnlyList<BatchRunRequest>> GetDuePlannedRunRequestsAsync(DateTime utcNow, CancellationToken cancellationToken);

    /// <summary>
    /// Gets active bindings for a schedule at a point in time.
    /// </summary>
    Task<IReadOnlyList<TenantJobScheduleBinding>> GetApplicableBindingsAsync(int batchScheduleId, DateTime utcNow, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a tenant by identifier.
    /// </summary>
    Task<Tenant?> GetTenantAsync(int tenantId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a batch job by identifier.
    /// </summary>
    Task<BatchJob?> GetBatchJobAsync(int batchJobId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a tenant job by tenant and job identifiers.
    /// </summary>
    Task<TenantJob?> GetTenantJobAsync(int tenantId, int batchJobId, CancellationToken cancellationToken);

    /// <summary>
    /// Attempts to add a job instance and returns whether it was inserted.
    /// </summary>
    Task<bool> TryAddJobInstanceAsync(BatchJobInstance instance, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paged list of job instances for a tenant.
    /// </summary>
    Task<PagedResult<BatchJobInstance>> GetInstancesAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a job instance by identifier.
    /// </summary>
    Task<BatchJobInstance?> GetInstanceAsync(Guid instanceId, CancellationToken cancellationToken);

    /// <summary>
    /// Persists changes to tracked orchestration entities.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
