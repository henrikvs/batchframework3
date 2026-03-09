using Batch.Domain.Contracts;
using Batch.Domain.Entities;

namespace Batch.DataAccess;

/// <summary>
/// Provides data access operations for configuration entities managed by API services.
/// </summary>
public interface IAdministrationDA
{
    /// <summary>
    /// Gets a paged list of tenants.
    /// </summary>
    Task<PagedResult<Tenant>> GetTenantsAsync(int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a tenant by identifier.
    /// </summary>
    Task<Tenant?> GetTenantAsync(int tenantId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a runner profile by identifier.
    /// </summary>
    Task<RunnerProfile?> GetRunnerProfileAsync(int runnerProfileId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paged list of runner profiles.
    /// </summary>
    Task<PagedResult<RunnerProfile>> GetRunnerProfilesAsync(int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a batch job by identifier.
    /// </summary>
    Task<BatchJob?> GetBatchJobAsync(int batchJobId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paged list of batch jobs.
    /// </summary>
    Task<PagedResult<BatchJob>> GetBatchJobsAsync(int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a tenant execution parameter by tenant and identifier.
    /// </summary>
    Task<TenantExecutionParameter?> GetTenantExecutionParameterAsync(int tenantId, int executionParameterId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paged list of tenant execution parameters for a tenant.
    /// </summary>
    Task<PagedResult<TenantExecutionParameter>> GetTenantExecutionParametersAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a tenant job by tenant and job identifiers.
    /// </summary>
    Task<TenantJob?> GetTenantJobAsync(int tenantId, int batchJobId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paged list of tenant jobs for a tenant.
    /// </summary>
    Task<PagedResult<TenantJob>> GetTenantJobsAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a batch schedule by identifier.
    /// </summary>
    Task<BatchSchedule?> GetBatchScheduleAsync(int batchScheduleId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paged list of schedules for a job.
    /// </summary>
    Task<PagedResult<BatchSchedule>> GetBatchSchedulesAsync(int batchJobId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paged list of schedule bindings for a tenant job.
    /// </summary>
    Task<PagedResult<TenantJobScheduleBinding>> GetScheduleBindingsAsync(int tenantId, int batchJobId, int page, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a tenant.
    /// </summary>
    Task AddTenantAsync(Tenant tenant, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a runner profile.
    /// </summary>
    Task AddRunnerProfileAsync(RunnerProfile runnerProfile, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a batch job.
    /// </summary>
    Task AddBatchJobAsync(BatchJob batchJob, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a tenant execution parameter.
    /// </summary>
    Task AddTenantExecutionParameterAsync(TenantExecutionParameter parameter, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a tenant job.
    /// </summary>
    Task AddTenantJobAsync(TenantJob tenantJob, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a batch schedule.
    /// </summary>
    Task AddBatchScheduleAsync(BatchSchedule batchSchedule, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a schedule binding.
    /// </summary>
    Task AddScheduleBindingAsync(TenantJobScheduleBinding binding, CancellationToken cancellationToken);

    /// <summary>
    /// Saves updates to an existing tracked tenant.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets holiday calendar entries for a date range.
    /// </summary>
    Task<IReadOnlyList<HolidayCalendarEntry>> GetHolidayCalendarEntriesAsync(string calendarCode, DateOnly fromInclusive, DateOnly toInclusive, CancellationToken cancellationToken);
}
