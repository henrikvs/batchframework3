using Batch.Domain.Contracts;
using Batch.Domain.Entities;

namespace Batch.Services;

/// <summary>
/// Provides validated CRUD operations for configuration entities.
/// </summary>
public interface IAdministrationService
{
    /// <summary>
    /// Gets tenants.
    /// </summary>
    Task<PagedResult<Tenant>> GetTenantsAsync(int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a tenant.
    /// </summary>
    Task<Tenant> CreateTenantAsync(TenantUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a tenant.
    /// </summary>
    Task<Tenant> UpdateTenantAsync(int tenantId, TenantUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Gets runner profiles.
    /// </summary>
    Task<PagedResult<RunnerProfile>> GetRunnerProfilesAsync(int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a runner profile.
    /// </summary>
    Task<RunnerProfile> CreateRunnerProfileAsync(RunnerProfileUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a runner profile.
    /// </summary>
    Task<RunnerProfile> UpdateRunnerProfileAsync(int runnerProfileId, RunnerProfileUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Gets batch jobs.
    /// </summary>
    Task<PagedResult<BatchJob>> GetBatchJobsAsync(int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a batch job.
    /// </summary>
    Task<BatchJob> CreateBatchJobAsync(BatchJobUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a batch job.
    /// </summary>
    Task<BatchJob> UpdateBatchJobAsync(int batchJobId, BatchJobUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Gets tenant jobs for a tenant.
    /// </summary>
    Task<PagedResult<TenantJob>> GetTenantJobsAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Creates or updates a tenant job.
    /// </summary>
    Task<TenantJob> UpsertTenantJobAsync(int tenantId, int batchJobId, TenantJobUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Gets tenant execution parameters.
    /// </summary>
    Task<PagedResult<TenantExecutionParameter>> GetTenantExecutionParametersAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a tenant execution parameter.
    /// </summary>
    Task<TenantExecutionParameter> CreateTenantExecutionParameterAsync(int tenantId, TenantExecutionParameterUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a tenant execution parameter.
    /// </summary>
    Task<TenantExecutionParameter> UpdateTenantExecutionParameterAsync(int tenantId, int executionParameterId, TenantExecutionParameterUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Gets schedules for a job.
    /// </summary>
    Task<PagedResult<BatchSchedule>> GetBatchSchedulesAsync(int batchJobId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a batch schedule.
    /// </summary>
    Task<BatchSchedule> CreateBatchScheduleAsync(int batchJobId, BatchScheduleUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a batch schedule.
    /// </summary>
    Task<BatchSchedule> UpdateBatchScheduleAsync(int batchScheduleId, BatchScheduleUpsertModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Gets schedule bindings for a tenant job.
    /// </summary>
    Task<PagedResult<TenantJobScheduleBinding>> GetScheduleBindingsAsync(int tenantId, int batchJobId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a schedule binding for a tenant job.
    /// </summary>
    Task<TenantJobScheduleBinding> CreateScheduleBindingAsync(int tenantId, int batchJobId, TenantJobScheduleBindingUpsertModel model, CancellationToken cancellationToken);
}
