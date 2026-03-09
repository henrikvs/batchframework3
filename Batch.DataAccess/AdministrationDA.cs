using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Batch.DataAccess;

/// <inheritdoc />
public sealed class AdministrationDA(BatchDbContext dbContext) : IAdministrationDA
{
    /// <inheritdoc />
    public Task<BatchJob?> GetBatchJobAsync(int batchJobId, CancellationToken cancellationToken) =>
        dbContext.BatchJobs.AsNoTracking().SingleOrDefaultAsync(x => x.Id == batchJobId, cancellationToken);

    /// <inheritdoc />
    public Task<PagedResult<BatchJob>> GetBatchJobsAsync(int page, int pageSize, CancellationToken cancellationToken) =>
        dbContext.BatchJobs.AsNoTracking().OrderBy(x => x.JobName).ToPagedResultAsync(page, pageSize, cancellationToken);

    /// <inheritdoc />
    public Task<BatchSchedule?> GetBatchScheduleAsync(int batchScheduleId, CancellationToken cancellationToken) =>
        dbContext.BatchSchedules.SingleOrDefaultAsync(x => x.Id == batchScheduleId, cancellationToken);

    /// <inheritdoc />
    public Task<PagedResult<BatchSchedule>> GetBatchSchedulesAsync(int batchJobId, int page, int pageSize, CancellationToken cancellationToken) =>
        dbContext.BatchSchedules
            .AsNoTracking()
            .Where(x => x.BatchJobId == batchJobId)
            .OrderBy(x => x.Name)
            .ToPagedResultAsync(page, pageSize, cancellationToken);

    /// <inheritdoc />
    public Task<RunnerProfile?> GetRunnerProfileAsync(int runnerProfileId, CancellationToken cancellationToken) =>
        dbContext.RunnerProfiles.SingleOrDefaultAsync(x => x.Id == runnerProfileId, cancellationToken);

    /// <inheritdoc />
    public Task<PagedResult<RunnerProfile>> GetRunnerProfilesAsync(int page, int pageSize, CancellationToken cancellationToken) =>
        dbContext.RunnerProfiles.AsNoTracking().OrderBy(x => x.Name).ToPagedResultAsync(page, pageSize, cancellationToken);

    /// <inheritdoc />
    public Task<Tenant?> GetTenantAsync(int tenantId, CancellationToken cancellationToken) =>
        dbContext.Tenants.SingleOrDefaultAsync(x => x.Id == tenantId, cancellationToken);

    /// <inheritdoc />
    public Task<PagedResult<Tenant>> GetTenantsAsync(int page, int pageSize, CancellationToken cancellationToken) =>
        dbContext.Tenants.AsNoTracking().OrderBy(x => x.Code).ToPagedResultAsync(page, pageSize, cancellationToken);

    /// <inheritdoc />
    public Task<TenantExecutionParameter?> GetTenantExecutionParameterAsync(int tenantId, int executionParameterId, CancellationToken cancellationToken) =>
        dbContext.TenantExecutionParameters.SingleOrDefaultAsync(
            x => x.TenantId == tenantId && x.Id == executionParameterId,
            cancellationToken);

    /// <inheritdoc />
    public Task<PagedResult<TenantExecutionParameter>> GetTenantExecutionParametersAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) =>
        dbContext.TenantExecutionParameters
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.Key)
            .ToPagedResultAsync(page, pageSize, cancellationToken);

    /// <inheritdoc />
    public Task<TenantJob?> GetTenantJobAsync(int tenantId, int batchJobId, CancellationToken cancellationToken) =>
        dbContext.TenantJobs.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.BatchJobId == batchJobId, cancellationToken);

    /// <inheritdoc />
    public Task<PagedResult<TenantJob>> GetTenantJobsAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) =>
        dbContext.TenantJobs
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.BatchJobId)
            .ToPagedResultAsync(page, pageSize, cancellationToken);

    /// <inheritdoc />
    public Task<PagedResult<TenantJobScheduleBinding>> GetScheduleBindingsAsync(int tenantId, int batchJobId, int page, int pageSize, CancellationToken cancellationToken) =>
        dbContext.TenantJobScheduleBindings
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.BatchJobId == batchJobId)
            .OrderBy(x => x.BatchScheduleId)
            .ToPagedResultAsync(page, pageSize, cancellationToken);

    /// <inheritdoc />
    public async Task AddTenantAsync(Tenant tenant, CancellationToken cancellationToken)
    {
        await dbContext.Tenants.AddAsync(tenant, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddRunnerProfileAsync(RunnerProfile runnerProfile, CancellationToken cancellationToken)
    {
        await dbContext.RunnerProfiles.AddAsync(runnerProfile, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddBatchJobAsync(BatchJob batchJob, CancellationToken cancellationToken)
    {
        await dbContext.BatchJobs.AddAsync(batchJob, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddTenantExecutionParameterAsync(TenantExecutionParameter parameter, CancellationToken cancellationToken)
    {
        await dbContext.TenantExecutionParameters.AddAsync(parameter, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddTenantJobAsync(TenantJob tenantJob, CancellationToken cancellationToken)
    {
        await dbContext.TenantJobs.AddAsync(tenantJob, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddBatchScheduleAsync(BatchSchedule batchSchedule, CancellationToken cancellationToken)
    {
        await dbContext.BatchSchedules.AddAsync(batchSchedule, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddScheduleBindingAsync(TenantJobScheduleBinding binding, CancellationToken cancellationToken)
    {
        await dbContext.TenantJobScheduleBindings.AddAsync(binding, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<HolidayCalendarEntry>> GetHolidayCalendarEntriesAsync(
        string calendarCode,
        DateOnly fromInclusive,
        DateOnly toInclusive,
        CancellationToken cancellationToken) =>
        await dbContext.HolidayCalendarEntries
            .AsNoTracking()
            .Where(x => x.Code == calendarCode && x.Date >= fromInclusive && x.Date <= toInclusive)
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);
}
