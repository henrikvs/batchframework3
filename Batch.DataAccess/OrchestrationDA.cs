using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Batch.DataAccess;

/// <inheritdoc />
public sealed class OrchestrationDA(BatchDbContext dbContext) : IOrchestrationDA
{
    /// <inheritdoc />
    public async Task AddRunRequestAsync(BatchRunRequest runRequest, CancellationToken cancellationToken)
    {
        await dbContext.BatchRunRequests.AddAsync(runRequest, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> TryAddScheduledRunRequestAsync(BatchRunRequest runRequest, CancellationToken cancellationToken)
    {
        var exists = await dbContext.BatchRunRequests.AnyAsync(
            x => x.BatchScheduleId == runRequest.BatchScheduleId && x.SlotKey == runRequest.SlotKey,
            cancellationToken);
        if (exists)
        {
            return false;
        }

        await dbContext.BatchRunRequests.AddAsync(runRequest, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public Task<BatchRunRequest?> GetRunRequestAsync(Guid runRequestId, CancellationToken cancellationToken) =>
        dbContext.BatchRunRequests.SingleOrDefaultAsync(x => x.Id == runRequestId, cancellationToken);

    /// <inheritdoc />
    public Task<PagedResult<BatchRunRequest>> GetRunRequestsAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) =>
        dbContext.BatchRunRequests
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.CreatedUtc)
            .ToPagedResultAsync(page, pageSize, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<BatchSchedule>> GetEnabledSchedulesAsync(CancellationToken cancellationToken) =>
        await dbContext.BatchSchedules
            .Where(x => x.IsEnabled)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<BatchRunRequest>> GetDuePlannedRunRequestsAsync(DateTime utcNow, CancellationToken cancellationToken) =>
        await dbContext.BatchRunRequests
            .Where(x => x.Status == BrqStatus.Planned && x.FireAtUtc <= utcNow)
            .OrderBy(x => x.FireAtUtc)
            .ThenBy(x => x.CreatedUtc)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<TenantJobScheduleBinding>> GetApplicableBindingsAsync(int batchScheduleId, DateTime utcNow, CancellationToken cancellationToken) =>
        await dbContext.TenantJobScheduleBindings
            .AsNoTracking()
            .Where(x => x.BatchScheduleId == batchScheduleId && x.IsEnabled)
            .Where(x => x.EffectiveFromUtc == null || x.EffectiveFromUtc <= utcNow)
            .Where(x => x.EffectiveToUtc == null || x.EffectiveToUtc >= utcNow)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public Task<Tenant?> GetTenantAsync(int tenantId, CancellationToken cancellationToken) =>
        dbContext.Tenants.AsNoTracking().SingleOrDefaultAsync(x => x.Id == tenantId, cancellationToken);

    /// <inheritdoc />
    public Task<BatchJob?> GetBatchJobAsync(int batchJobId, CancellationToken cancellationToken) =>
        dbContext.BatchJobs.AsNoTracking().SingleOrDefaultAsync(x => x.Id == batchJobId, cancellationToken);

    /// <inheritdoc />
    public Task<TenantJob?> GetTenantJobAsync(int tenantId, int batchJobId, CancellationToken cancellationToken) =>
        dbContext.TenantJobs.AsNoTracking().SingleOrDefaultAsync(x => x.TenantId == tenantId && x.BatchJobId == batchJobId, cancellationToken);

    /// <inheritdoc />
    public async Task<bool> TryAddJobInstanceAsync(BatchJobInstance instance, CancellationToken cancellationToken)
    {
        var exists = await dbContext.BatchJobInstances.AnyAsync(
            x => x.TenantId == instance.TenantId
                && x.BatchJobId == instance.BatchJobId
                && x.BatchScheduleId == instance.BatchScheduleId
                && x.OccurrenceKey == instance.OccurrenceKey
                && x.ScopeKey == instance.ScopeKey,
            cancellationToken);
        if (exists)
        {
            return false;
        }

        await dbContext.BatchJobInstances.AddAsync(instance, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public Task<PagedResult<BatchJobInstance>> GetInstancesAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) =>
        dbContext.BatchJobInstances
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.CreatedUtc)
            .ToPagedResultAsync(page, pageSize, cancellationToken);

    /// <inheritdoc />
    public Task<BatchJobInstance?> GetInstanceAsync(Guid instanceId, CancellationToken cancellationToken) =>
        dbContext.BatchJobInstances.AsNoTracking().SingleOrDefaultAsync(x => x.Id == instanceId, cancellationToken);

    /// <inheritdoc />
    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
