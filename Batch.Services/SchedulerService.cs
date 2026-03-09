using Batch.DataAccess;
using Batch.Domain.Entities;
using Batch.Domain.Enums;
using Batch.Services.Scheduling;

namespace Batch.Services;

/// <inheritdoc />
public sealed class SchedulerService(IOrchestrationDA orchestrationDA, ScheduleCalculator scheduleCalculator, TimeProvider timeProvider) : ISchedulerService
{
    private static readonly TimeSpan SchedulerLookahead = TimeSpan.FromMinutes(5);

    /// <inheritdoc />
    public async Task MaterializeScheduledRunRequestsAsync(CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var lookaheadLimit = now.Add(SchedulerLookahead);
        var schedules = await orchestrationDA.GetEnabledSchedulesAsync(cancellationToken);

        foreach (var schedule in schedules)
        {
            schedule.NextFireUtc ??= await scheduleCalculator.GetNextOccurrenceUtcAsync(schedule, now.AddSeconds(-1), cancellationToken);
            if (!schedule.NextFireUtc.HasValue)
            {
                continue;
            }

            if (schedule.NextFireUtc.Value < now)
            {
                await HandleMisfireAsync(schedule, now, cancellationToken);
            }

            while (schedule.NextFireUtc.HasValue && schedule.NextFireUtc.Value <= lookaheadLimit)
            {
                await CreateScheduledRunRequestAsync(schedule, schedule.NextFireUtc.Value, schedule.NextFireUtc.Value, cancellationToken);
                schedule.NextFireUtc = await scheduleCalculator.GetNextOccurrenceUtcAsync(schedule, schedule.NextFireUtc.Value, cancellationToken);
            }

            schedule.UpdatedUtc = now;
            await orchestrationDA.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task HandleMisfireAsync(BatchSchedule schedule, DateTime now, CancellationToken cancellationToken)
    {
        if (schedule.MisfirePolicy == BscMisfirePolicy.Skip)
        {
            while (schedule.NextFireUtc.HasValue && schedule.NextFireUtc.Value < now)
            {
                schedule.NextFireUtc = await scheduleCalculator.GetNextOccurrenceUtcAsync(schedule, schedule.NextFireUtc.Value, cancellationToken);
            }

            return;
        }

        var missed = schedule.NextFireUtc;
        while (schedule.NextFireUtc.HasValue && schedule.NextFireUtc.Value < now)
        {
            missed = schedule.NextFireUtc;
            schedule.NextFireUtc = await scheduleCalculator.GetNextOccurrenceUtcAsync(schedule, schedule.NextFireUtc.Value, cancellationToken);
        }

        if (missed.HasValue)
        {
            await CreateScheduledRunRequestAsync(schedule, missed.Value, now, cancellationToken);
        }
    }

    private async Task CreateScheduledRunRequestAsync(BatchSchedule schedule, DateTime slotTimeUtc, DateTime fireAtUtc, CancellationToken cancellationToken)
    {
        var slotKey = scheduleCalculator.BuildSlotKey(schedule, slotTimeUtc);
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var runRequest = new BatchRunRequest
        {
            Id = Guid.NewGuid(),
            TriggerType = BrqTriggerType.Schedule,
            BatchScheduleId = schedule.Id,
            SlotKey = slotKey,
            FireAtUtc = fireAtUtc,
            IncludePrerequisites = false,
            Mode = BrqMode.SingleJob,
            ScopeKey = string.Empty,
            RespectDependencies = false,
            RespectGates = true,
            OnlyIfReady = false,
            ForceGates = false,
            Status = BrqStatus.Planned,
            CreatedBy = "scheduler",
            CreatedUtc = now,
            UpdatedUtc = now,
        };

        _ = await orchestrationDA.TryAddScheduledRunRequestAsync(runRequest, cancellationToken);
        schedule.LastSlotKey = slotKey;
    }
}
