using Batch.DataAccess;
using Batch.Domain.Entities;
using Batch.Domain.Enums;

namespace Batch.Services;

/// <inheritdoc />
public sealed class RequestFiringService(IOrchestrationDA orchestrationDA, TimeProvider timeProvider) : IRequestFiringService
{
    /// <inheritdoc />
    public async Task FireDueRunRequestsAsync(CancellationToken cancellationToken)
    {
        var dueRequests = await orchestrationDA.GetDuePlannedRunRequestsAsync(timeProvider.GetUtcNow().UtcDateTime, cancellationToken);
        foreach (var request in dueRequests)
        {
            if (request.TriggerType == BrqTriggerType.Schedule && request.BatchScheduleId.HasValue)
            {
                await FireScheduledRequestAsync(request, cancellationToken);
            }
            else
            {
                await FireManualRequestAsync(request, cancellationToken);
            }
        }
    }

    private async Task FireScheduledRequestAsync(BatchRunRequest request, CancellationToken cancellationToken)
    {
        var bindings = await orchestrationDA.GetApplicableBindingsAsync(request.BatchScheduleId!.Value, request.FireAtUtc, cancellationToken);
        foreach (var binding in bindings)
        {
            var tenant = await orchestrationDA.GetTenantAsync(binding.TenantId, cancellationToken);
            var tenantJob = await orchestrationDA.GetTenantJobAsync(binding.TenantId, binding.BatchJobId, cancellationToken);
            if (tenant?.Status != TenStatus.Active || tenantJob?.IsEnabled != true)
            {
                continue;
            }

            await orchestrationDA.TryAddJobInstanceAsync(
                CreateInstance(
                    request,
                    binding.TenantId,
                    binding.BatchJobId,
                    request.BatchScheduleId,
                    request.SlotKey ?? request.Id.ToString(),
                    string.Empty),
                cancellationToken);
        }

        request.Status = BrqStatus.Fired;
        request.UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime;
        await orchestrationDA.SaveChangesAsync(cancellationToken);
    }

    private async Task FireManualRequestAsync(BatchRunRequest request, CancellationToken cancellationToken)
    {
        var tenant = await orchestrationDA.GetTenantAsync(request.TenantId!.Value, cancellationToken);
        var batchJob = await orchestrationDA.GetBatchJobAsync(request.BatchJobId!.Value, cancellationToken);
        var tenantJob = await orchestrationDA.GetTenantJobAsync(request.TenantId.Value, request.BatchJobId.Value, cancellationToken);
        if (tenant?.Status != TenStatus.Active || batchJob is null || tenantJob?.IsEnabled != true)
        {
            request.Status = BrqStatus.Failed;
            request.Reason = "The run request could not be fired because the tenant or tenant-job is not active.";
            request.UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime;
            await orchestrationDA.SaveChangesAsync(cancellationToken);
            return;
        }

        _ = await orchestrationDA.TryAddJobInstanceAsync(
            CreateInstance(request, request.TenantId.Value, request.BatchJobId.Value, null, request.Id.ToString(), request.ScopeKey),
            cancellationToken);

        request.Status = BrqStatus.Fired;
        request.UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime;
        await orchestrationDA.SaveChangesAsync(cancellationToken);
    }

    private BatchJobInstance CreateInstance(
        BatchRunRequest request,
        int tenantId,
        int batchJobId,
        int? batchScheduleId,
        string occurrenceKey,
        string scopeKey)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        return new BatchJobInstance
        {
            Id = Guid.NewGuid(),
            BatchRunRequestId = request.Id,
            TenantId = tenantId,
            BatchJobId = batchJobId,
            BatchScheduleId = batchScheduleId,
            OccurrenceKey = occurrenceKey,
            ScopeKey = scopeKey,
            ExecutionContextJson = request.ExecutionContextJson,
            PlannedStartUtc = request.FireAtUtc,
            Status = BinStatus.Planned,
            AttemptNumber = 0,
            CreatedUtc = now,
            UpdatedUtc = now,
        };
    }
}
