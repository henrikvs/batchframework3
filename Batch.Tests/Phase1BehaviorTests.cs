using System.ComponentModel.DataAnnotations;
using Batch.DataAccess;
using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Domain.Enums;
using Batch.Services;
using Batch.Services.Scheduling;

namespace Batch.Tests;

/// <summary>
/// Validates the Phase 1 behaviors implemented from the specification.
/// </summary>
public sealed class Phase1BehaviorTests
{
    /// <summary>
    /// Verifies that manual run creation defaults the scope key to the tenant-wide empty string.
    /// </summary>
    [Fact]
    public async Task CreateManualRunRequestAsync_DefaultsScopeKeyToEmptyString()
    {
        var administrationDA = new FakeAdministrationDA
        {
            Tenant = new Tenant { Id = 7, Status = TenStatus.Active, TimeZone = "UTC", Code = "TST", Name = "Test" },
            BatchJob = new BatchJob { Id = 11, JobName = "ImportTrades" },
            TenantJob = new TenantJob { TenantId = 7, BatchJobId = 11, IsEnabled = true },
        };
        var orchestrationDA = new FakeOrchestrationDA();
        var service = new RunRequestService(administrationDA, orchestrationDA, new FixedTimeProvider(new DateTime(2026, 3, 9, 1, 0, 0, DateTimeKind.Utc)));

        var result = await service.CreateManualRunRequestAsync(7, new ManualRunRequestModel { BatchJobId = 11 }, CancellationToken.None);

        Assert.Equal(string.Empty, result.ScopeKey);
        Assert.Equal(BrqMode.SingleJob, result.Mode);
        Assert.Single(orchestrationDA.AddedRunRequests);
    }

    /// <summary>
    /// Verifies that invalid repeat-in-window configuration is rejected by service validation.
    /// </summary>
    [Fact]
    public async Task CreateBatchScheduleAsync_RejectsInvalidRepeatInWindowConfiguration()
    {
        var administrationDA = new FakeAdministrationDA
        {
            BatchJob = new BatchJob { Id = 3, JobName = "DailyImport" },
        };
        var scheduleCalculator = new ScheduleCalculator(administrationDA);
        var service = new AdministrationService(administrationDA, scheduleCalculator, new FixedTimeProvider(new DateTime(2026, 3, 9, 1, 0, 0, DateTimeKind.Utc)));

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateBatchScheduleAsync(
            3,
            new BatchScheduleUpsertModel
            {
                Name = "Broken",
                ScheduleType = BscScheduleType.RepeatInWindow,
                TimeZone = "UTC",
                ConfigJson = "{\"windowStart\":\"12:00:00\",\"windowEnd\":\"08:00:00\",\"intervalMinutes\":15}",
                MisfirePolicy = BscMisfirePolicy.Skip,
                IsEnabled = true,
            },
            CancellationToken.None));
    }

    /// <summary>
    /// Verifies that firing a due manual run request creates one planned instance and marks the request as fired.
    /// </summary>
    [Fact]
    public async Task FireDueRunRequestsAsync_CreatesManualInstanceAndMarksRequestFired()
    {
        var request = new BatchRunRequest
        {
            Id = Guid.NewGuid(),
            TenantId = 12,
            BatchJobId = 99,
            TriggerType = BrqTriggerType.Api,
            FireAtUtc = new DateTime(2026, 3, 9, 1, 0, 0, DateTimeKind.Utc),
            ScopeKey = string.Empty,
            Status = BrqStatus.Planned,
            CreatedBy = "test",
            CreatedUtc = new DateTime(2026, 3, 9, 1, 0, 0, DateTimeKind.Utc),
            UpdatedUtc = new DateTime(2026, 3, 9, 1, 0, 0, DateTimeKind.Utc),
        };
        var orchestrationDA = new FakeOrchestrationDA
        {
            DueRequests = [request],
            Tenant = new Tenant { Id = 12, Status = TenStatus.Active, TimeZone = "UTC", Code = "TEN", Name = "Tenant" },
            BatchJob = new BatchJob { Id = 99, JobName = "ImportTrades" },
            TenantJob = new TenantJob { TenantId = 12, BatchJobId = 99, IsEnabled = true },
        };
        var service = new RequestFiringService(orchestrationDA, new FixedTimeProvider(new DateTime(2026, 3, 9, 1, 5, 0, DateTimeKind.Utc)));

        await service.FireDueRunRequestsAsync(CancellationToken.None);

        Assert.Equal(BrqStatus.Fired, request.Status);
        Assert.Single(orchestrationDA.AddedInstances);
        Assert.Equal(request.Id.ToString(), orchestrationDA.AddedInstances[0].OccurrenceKey);
    }

    private sealed class FixedTimeProvider(DateTime utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => new(utcNow, TimeSpan.Zero);
    }

    private sealed class FakeAdministrationDA : IAdministrationDA
    {
        public Tenant? Tenant { get; set; }

        public RunnerProfile? RunnerProfile { get; set; }

        public BatchJob? BatchJob { get; set; }

        public TenantJob? TenantJob { get; set; }

        public List<BatchSchedule> AddedSchedules { get; } = [];

        public Task AddBatchJobAsync(BatchJob batchJob, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task AddBatchScheduleAsync(BatchSchedule batchSchedule, CancellationToken cancellationToken)
        {
            AddedSchedules.Add(batchSchedule);
            return Task.CompletedTask;
        }
        public Task AddRunnerProfileAsync(RunnerProfile runnerProfile, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task AddScheduleBindingAsync(TenantJobScheduleBinding binding, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task AddTenantAsync(Tenant tenant, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task AddTenantExecutionParameterAsync(TenantExecutionParameter parameter, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task AddTenantJobAsync(TenantJob tenantJob, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<BatchJob?> GetBatchJobAsync(int batchJobId, CancellationToken cancellationToken) => Task.FromResult(BatchJob?.Id == batchJobId ? BatchJob : null);
        public Task<PagedResult<BatchJob>> GetBatchJobsAsync(int page, int pageSize, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<BatchSchedule?> GetBatchScheduleAsync(int batchScheduleId, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<PagedResult<BatchSchedule>> GetBatchSchedulesAsync(int batchJobId, int page, int pageSize, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<IReadOnlyList<HolidayCalendarEntry>> GetHolidayCalendarEntriesAsync(string calendarCode, DateOnly fromInclusive, DateOnly toInclusive, CancellationToken cancellationToken) => Task.FromResult<IReadOnlyList<HolidayCalendarEntry>>([]);
        public Task<RunnerProfile?> GetRunnerProfileAsync(int runnerProfileId, CancellationToken cancellationToken) => Task.FromResult(RunnerProfile?.Id == runnerProfileId ? RunnerProfile : null);
        public Task<PagedResult<RunnerProfile>> GetRunnerProfilesAsync(int page, int pageSize, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<PagedResult<TenantJobScheduleBinding>> GetScheduleBindingsAsync(int tenantId, int batchJobId, int page, int pageSize, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<Tenant?> GetTenantAsync(int tenantId, CancellationToken cancellationToken) => Task.FromResult(Tenant?.Id == tenantId ? Tenant : null);
        public Task<PagedResult<Tenant>> GetTenantsAsync(int page, int pageSize, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<TenantExecutionParameter?> GetTenantExecutionParameterAsync(int tenantId, int executionParameterId, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<PagedResult<TenantExecutionParameter>> GetTenantExecutionParametersAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<TenantJob?> GetTenantJobAsync(int tenantId, int batchJobId, CancellationToken cancellationToken) => Task.FromResult(TenantJob?.TenantId == tenantId && TenantJob.BatchJobId == batchJobId ? TenantJob : null);
        public Task<PagedResult<TenantJob>> GetTenantJobsAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task SaveChangesAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class FakeOrchestrationDA : IOrchestrationDA
    {
        public List<BatchRunRequest> AddedRunRequests { get; } = [];
        public List<BatchJobInstance> AddedInstances { get; } = [];
        public IReadOnlyList<BatchRunRequest> DueRequests { get; set; } = [];
        public Tenant? Tenant { get; set; }
        public BatchJob? BatchJob { get; set; }
        public TenantJob? TenantJob { get; set; }

        public Task AddRunRequestAsync(BatchRunRequest runRequest, CancellationToken cancellationToken)
        {
            AddedRunRequests.Add(runRequest);
            return Task.CompletedTask;
        }

        public Task<BatchJob?> GetBatchJobAsync(int batchJobId, CancellationToken cancellationToken) => Task.FromResult(BatchJob?.Id == batchJobId ? BatchJob : null);
        public Task<IReadOnlyList<TenantJobScheduleBinding>> GetApplicableBindingsAsync(int batchScheduleId, DateTime utcNow, CancellationToken cancellationToken) => Task.FromResult<IReadOnlyList<TenantJobScheduleBinding>>([]);
        public Task<IReadOnlyList<BatchRunRequest>> GetDuePlannedRunRequestsAsync(DateTime utcNow, CancellationToken cancellationToken) => Task.FromResult(DueRequests);
        public Task<IReadOnlyList<BatchSchedule>> GetEnabledSchedulesAsync(CancellationToken cancellationToken) => Task.FromResult<IReadOnlyList<BatchSchedule>>([]);
        public Task<BatchJobInstance?> GetInstanceAsync(Guid instanceId, CancellationToken cancellationToken) => Task.FromResult<BatchJobInstance?>(null);
        public Task<PagedResult<BatchJobInstance>> GetInstancesAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<BatchRunRequest?> GetRunRequestAsync(Guid runRequestId, CancellationToken cancellationToken) => Task.FromResult<BatchRunRequest?>(null);
        public Task<PagedResult<BatchRunRequest>> GetRunRequestsAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<Tenant?> GetTenantAsync(int tenantId, CancellationToken cancellationToken) => Task.FromResult(Tenant?.Id == tenantId ? Tenant : null);
        public Task<TenantJob?> GetTenantJobAsync(int tenantId, int batchJobId, CancellationToken cancellationToken) => Task.FromResult(TenantJob?.TenantId == tenantId && TenantJob.BatchJobId == batchJobId ? TenantJob : null);
        public Task SaveChangesAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task<bool> TryAddJobInstanceAsync(BatchJobInstance instance, CancellationToken cancellationToken)
        {
            AddedInstances.Add(instance);
            return Task.FromResult(true);
        }
        public Task<bool> TryAddScheduledRunRequestAsync(BatchRunRequest runRequest, CancellationToken cancellationToken)
        {
            AddedRunRequests.Add(runRequest);
            return Task.FromResult(true);
        }
    }
}
