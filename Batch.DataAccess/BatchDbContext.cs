using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Batch.DataAccess;

/// <summary>
/// Provides Entity Framework Core access to the batch framework schema.
/// </summary>
public sealed class BatchDbContext(DbContextOptions<BatchDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets the tenants.
    /// </summary>
    public DbSet<Tenant> Tenants => Set<Tenant>();

    /// <summary>
    /// Gets the runner profiles.
    /// </summary>
    public DbSet<RunnerProfile> RunnerProfiles => Set<RunnerProfile>();

    /// <summary>
    /// Gets the batch jobs.
    /// </summary>
    public DbSet<BatchJob> BatchJobs => Set<BatchJob>();

    /// <summary>
    /// Gets the tenant execution parameters.
    /// </summary>
    public DbSet<TenantExecutionParameter> TenantExecutionParameters => Set<TenantExecutionParameter>();

    /// <summary>
    /// Gets the tenant jobs.
    /// </summary>
    public DbSet<TenantJob> TenantJobs => Set<TenantJob>();

    /// <summary>
    /// Gets the batch schedules.
    /// </summary>
    public DbSet<BatchSchedule> BatchSchedules => Set<BatchSchedule>();

    /// <summary>
    /// Gets the tenant job schedule bindings.
    /// </summary>
    public DbSet<TenantJobScheduleBinding> TenantJobScheduleBindings => Set<TenantJobScheduleBinding>();

    /// <summary>
    /// Gets the batch run requests.
    /// </summary>
    public DbSet<BatchRunRequest> BatchRunRequests => Set<BatchRunRequest>();

    /// <summary>
    /// Gets the batch job instances.
    /// </summary>
    public DbSet<BatchJobInstance> BatchJobInstances => Set<BatchJobInstance>();

    /// <summary>
    /// Gets the holiday calendar entries.
    /// </summary>
    public DbSet<HolidayCalendarEntry> HolidayCalendarEntries => Set<HolidayCalendarEntry>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BatchDbContext).Assembly);
    }
}
