using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Batch.DataAccess;
using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Domain.Enums;

namespace Batch.Services;

/// <inheritdoc />
public sealed class RunRequestService(IAdministrationDA administrationDA, IOrchestrationDA orchestrationDA, TimeProvider timeProvider) : IRunRequestService
{
    /// <inheritdoc />
    public async Task<BatchRunRequest> CreateManualRunRequestAsync(int tenantId, ManualRunRequestModel model, CancellationToken cancellationToken)
    {
        var tenant = await administrationDA.GetTenantAsync(tenantId, cancellationToken) ?? throw new KeyNotFoundException($"Tenant {tenantId} was not found.");
        if (tenant.Status != TenStatus.Active)
        {
            throw new ValidationException("Manual requests may only target active tenants.");
        }

        _ = await administrationDA.GetBatchJobAsync(model.BatchJobId, cancellationToken) ?? throw new KeyNotFoundException($"Batch job {model.BatchJobId} was not found.");
        var tenantJob = await administrationDA.GetTenantJobAsync(tenantId, model.BatchJobId, cancellationToken)
            ?? throw new ValidationException("Manual requests require an existing tenant-job configuration.");
        if (!tenantJob.IsEnabled)
        {
            throw new ValidationException("Manual requests must target an enabled tenant-job configuration.");
        }

        if (model.IncludePrerequisites)
        {
            throw new ValidationException("Phase 1 does not support IncludePrerequisites = true.");
        }

        if (!string.IsNullOrWhiteSpace(model.ExecutionContextJson))
        {
            JsonDocument.Parse(model.ExecutionContextJson);
        }

        var now = timeProvider.GetUtcNow().UtcDateTime;
        var runRequest = new BatchRunRequest
        {
            Id = Guid.NewGuid(),
            TriggerType = BrqTriggerType.Api,
            TenantId = tenantId,
            BatchJobId = model.BatchJobId,
            BatchScheduleId = null,
            SlotKey = null,
            FireAtUtc = now,
            IncludePrerequisites = false,
            Mode = BrqMode.SingleJob,
            TargetTenantFilterJson = null,
            ExecutionContextJson = string.IsNullOrWhiteSpace(model.ExecutionContextJson) ? null : model.ExecutionContextJson,
            ScopeKey = model.ScopeKey ?? string.Empty,
            RespectDependencies = false,
            RespectGates = true,
            OnlyIfReady = false,
            ForceGates = model.ForceGates,
            Status = BrqStatus.Planned,
            Reason = null,
            CreatedBy = string.IsNullOrWhiteSpace(model.CreatedBy) ? "api" : model.CreatedBy.Trim(),
            CreatedUtc = now,
            UpdatedUtc = now,
        };

        await orchestrationDA.AddRunRequestAsync(runRequest, cancellationToken);
        return runRequest;
    }

    /// <inheritdoc />
    public Task<PagedResult<BatchRunRequest>> GetRunRequestsAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) =>
        orchestrationDA.GetRunRequestsAsync(tenantId, page < 1 ? 1 : page, pageSize <= 0 ? 50 : pageSize, cancellationToken);

    /// <inheritdoc />
    public async Task<BatchRunRequest> CancelRunRequestAsync(Guid runRequestId, CancellationToken cancellationToken)
    {
        var runRequest = await orchestrationDA.GetRunRequestAsync(runRequestId, cancellationToken)
            ?? throw new KeyNotFoundException($"Run request {runRequestId} was not found.");
        if (runRequest.Status != BrqStatus.Planned)
        {
            throw new ValidationException("Only planned run requests can be cancelled.");
        }

        runRequest.Status = BrqStatus.Cancelled;
        runRequest.UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime;
        await orchestrationDA.SaveChangesAsync(cancellationToken);
        return runRequest;
    }

    /// <inheritdoc />
    public async Task<BatchRunRequest> RetryInstanceAsync(Guid instanceId, RetryRunRequestModel model, CancellationToken cancellationToken)
    {
        var instance = await orchestrationDA.GetInstanceAsync(instanceId, cancellationToken)
            ?? throw new KeyNotFoundException($"Instance {instanceId} was not found.");
        if (instance.Status is not (BinStatus.Failed or BinStatus.Cancelled))
        {
            throw new ValidationException("Only failed or cancelled instances can be retried.");
        }

        if (!string.IsNullOrWhiteSpace(model.ExecutionContextJson))
        {
            JsonDocument.Parse(model.ExecutionContextJson);
        }

        var now = timeProvider.GetUtcNow().UtcDateTime;
        var runRequest = new BatchRunRequest
        {
            Id = Guid.NewGuid(),
            TriggerType = BrqTriggerType.Api,
            TenantId = instance.TenantId,
            BatchJobId = instance.BatchJobId,
            FireAtUtc = now,
            IncludePrerequisites = false,
            Mode = BrqMode.SingleJob,
            ScopeKey = instance.ScopeKey,
            RespectDependencies = false,
            RespectGates = true,
            OnlyIfReady = false,
            ForceGates = false,
            Status = BrqStatus.Planned,
            CreatedBy = string.IsNullOrWhiteSpace(model.CreatedBy) ? "api-retry" : model.CreatedBy.Trim(),
            CreatedUtc = now,
            UpdatedUtc = now,
            ExecutionContextJson = string.IsNullOrWhiteSpace(model.ExecutionContextJson) ? instance.ExecutionContextJson : model.ExecutionContextJson,
        };

        await orchestrationDA.AddRunRequestAsync(runRequest, cancellationToken);
        return runRequest;
    }
}
