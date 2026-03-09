using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Batch.DataAccess;
using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Domain.Enums;
using Batch.Services.Scheduling;

namespace Batch.Services;

/// <inheritdoc />
public sealed class AdministrationService(
    IAdministrationDA administrationDA,
    ScheduleCalculator scheduleCalculator,
    TimeProvider timeProvider) : IAdministrationService
{
    /// <inheritdoc />
    public Task<PagedResult<Tenant>> GetTenantsAsync(int page, int pageSize, CancellationToken cancellationToken) =>
        administrationDA.GetTenantsAsync(NormalizePage(page), NormalizePageSize(pageSize), cancellationToken);

    /// <inheritdoc />
    public async Task<Tenant> CreateTenantAsync(TenantUpsertModel model, CancellationToken cancellationToken)
    {
        ValidateTenant(model);
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var tenant = new Tenant
        {
            Code = model.Code.Trim(),
            Name = model.Name.Trim(),
            Status = model.Status,
            TimeZone = model.TimeZone.Trim(),
            RootPath = NormalizeOptional(model.RootPath),
            Tags = NormalizeOptional(model.Tags),
            CreatedUtc = now,
            UpdatedUtc = now,
        };

        await administrationDA.AddTenantAsync(tenant, cancellationToken);
        return tenant;
    }

    /// <inheritdoc />
    public async Task<Tenant> UpdateTenantAsync(int tenantId, TenantUpsertModel model, CancellationToken cancellationToken)
    {
        ValidateTenant(model);
        var tenant = await administrationDA.GetTenantAsync(tenantId, cancellationToken) ?? throw new KeyNotFoundException($"Tenant {tenantId} was not found.");
        tenant.Code = model.Code.Trim();
        tenant.Name = model.Name.Trim();
        tenant.Status = model.Status;
        tenant.TimeZone = model.TimeZone.Trim();
        tenant.RootPath = NormalizeOptional(model.RootPath);
        tenant.Tags = NormalizeOptional(model.Tags);
        tenant.UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime;
        await administrationDA.SaveChangesAsync(cancellationToken);
        return tenant;
    }

    /// <inheritdoc />
    public Task<PagedResult<RunnerProfile>> GetRunnerProfilesAsync(int page, int pageSize, CancellationToken cancellationToken) =>
        administrationDA.GetRunnerProfilesAsync(NormalizePage(page), NormalizePageSize(pageSize), cancellationToken);

    /// <inheritdoc />
    public async Task<RunnerProfile> CreateRunnerProfileAsync(RunnerProfileUpsertModel model, CancellationToken cancellationToken)
    {
        ValidateRunnerProfile(model);
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var runnerProfile = new RunnerProfile
        {
            Name = model.Name.Trim(),
            Description = NormalizeOptional(model.Description),
            RunnerType = model.RunnerType,
            ProfileMode = model.ProfileMode,
            ExecutableTemplate = model.ExecutableTemplate.Trim(),
            WorkDirectoryTemplate = NormalizeOptional(model.WorkDirectoryTemplate),
            ArgumentTemplate = NormalizeOptional(model.ArgumentTemplate),
            ArgumentJoiner = NormalizeOptional(model.ArgumentJoiner),
            IsEnabled = model.IsEnabled,
            CreatedUtc = now,
            UpdatedUtc = now,
        };

        await administrationDA.AddRunnerProfileAsync(runnerProfile, cancellationToken);
        return runnerProfile;
    }

    /// <inheritdoc />
    public async Task<RunnerProfile> UpdateRunnerProfileAsync(int runnerProfileId, RunnerProfileUpsertModel model, CancellationToken cancellationToken)
    {
        ValidateRunnerProfile(model);
        var runnerProfile = await administrationDA.GetRunnerProfileAsync(runnerProfileId, cancellationToken)
            ?? throw new KeyNotFoundException($"Runner profile {runnerProfileId} was not found.");

        runnerProfile.Name = model.Name.Trim();
        runnerProfile.Description = NormalizeOptional(model.Description);
        runnerProfile.RunnerType = model.RunnerType;
        runnerProfile.ProfileMode = model.ProfileMode;
        runnerProfile.ExecutableTemplate = model.ExecutableTemplate.Trim();
        runnerProfile.WorkDirectoryTemplate = NormalizeOptional(model.WorkDirectoryTemplate);
        runnerProfile.ArgumentTemplate = NormalizeOptional(model.ArgumentTemplate);
        runnerProfile.ArgumentJoiner = NormalizeOptional(model.ArgumentJoiner);
        runnerProfile.IsEnabled = model.IsEnabled;
        runnerProfile.UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime;
        await administrationDA.SaveChangesAsync(cancellationToken);
        return runnerProfile;
    }

    /// <inheritdoc />
    public Task<PagedResult<BatchJob>> GetBatchJobsAsync(int page, int pageSize, CancellationToken cancellationToken) =>
        administrationDA.GetBatchJobsAsync(NormalizePage(page), NormalizePageSize(pageSize), cancellationToken);

    /// <inheritdoc />
    public async Task<BatchJob> CreateBatchJobAsync(BatchJobUpsertModel model, CancellationToken cancellationToken)
    {
        await ValidateBatchJobAsync(model, cancellationToken);
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var batchJob = CreateBatchJob(model, now);
        await administrationDA.AddBatchJobAsync(batchJob, cancellationToken);
        return batchJob;
    }

    /// <inheritdoc />
    public async Task<BatchJob> UpdateBatchJobAsync(int batchJobId, BatchJobUpsertModel model, CancellationToken cancellationToken)
    {
        await ValidateBatchJobAsync(model, cancellationToken);
        var batchJob = await administrationDA.GetBatchJobAsync(batchJobId, cancellationToken)
            ?? throw new KeyNotFoundException($"Batch job {batchJobId} was not found.");

        ApplyBatchJob(batchJob, model);
        batchJob.UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime;
        await administrationDA.SaveChangesAsync(cancellationToken);
        return batchJob;
    }

    /// <inheritdoc />
    public Task<PagedResult<TenantJob>> GetTenantJobsAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) =>
        administrationDA.GetTenantJobsAsync(tenantId, NormalizePage(page), NormalizePageSize(pageSize), cancellationToken);

    /// <inheritdoc />
    public async Task<TenantJob> UpsertTenantJobAsync(int tenantId, int batchJobId, TenantJobUpsertModel model, CancellationToken cancellationToken)
    {
        _ = await administrationDA.GetTenantAsync(tenantId, cancellationToken) ?? throw new KeyNotFoundException($"Tenant {tenantId} was not found.");
        _ = await administrationDA.GetBatchJobAsync(batchJobId, cancellationToken) ?? throw new KeyNotFoundException($"Batch job {batchJobId} was not found.");
        ValidateTenantJob(model);

        var tenantJob = await administrationDA.GetTenantJobAsync(tenantId, batchJobId, cancellationToken);
        var now = timeProvider.GetUtcNow().UtcDateTime;
        if (tenantJob is null)
        {
            tenantJob = new TenantJob
            {
                TenantId = tenantId,
                BatchJobId = batchJobId,
                IsEnabled = model.IsEnabled,
                ParameterJson = NormalizeOptional(model.ParameterJson),
                Priority = model.Priority,
                UpdatedBy = NormalizeOptional(model.UpdatedBy),
                UpdatedUtc = now,
            };

            await administrationDA.AddTenantJobAsync(tenantJob, cancellationToken);
            return tenantJob;
        }

        tenantJob.IsEnabled = model.IsEnabled;
        tenantJob.ParameterJson = NormalizeOptional(model.ParameterJson);
        tenantJob.Priority = model.Priority;
        tenantJob.UpdatedBy = NormalizeOptional(model.UpdatedBy);
        tenantJob.UpdatedUtc = now;
        await administrationDA.SaveChangesAsync(cancellationToken);
        return tenantJob;
    }

    /// <inheritdoc />
    public Task<PagedResult<TenantExecutionParameter>> GetTenantExecutionParametersAsync(int tenantId, int page, int pageSize, CancellationToken cancellationToken) =>
        administrationDA.GetTenantExecutionParametersAsync(tenantId, NormalizePage(page), NormalizePageSize(pageSize), cancellationToken);

    /// <inheritdoc />
    public async Task<TenantExecutionParameter> CreateTenantExecutionParameterAsync(int tenantId, TenantExecutionParameterUpsertModel model, CancellationToken cancellationToken)
    {
        _ = await administrationDA.GetTenantAsync(tenantId, cancellationToken) ?? throw new KeyNotFoundException($"Tenant {tenantId} was not found.");
        ValidateTenantExecutionParameter(model);
        var parameter = new TenantExecutionParameter
        {
            TenantId = tenantId,
            Key = model.Key.Trim(),
            Value = model.Value,
            UpdatedBy = NormalizeOptional(model.UpdatedBy),
            UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime,
        };
        await administrationDA.AddTenantExecutionParameterAsync(parameter, cancellationToken);
        return parameter;
    }

    /// <inheritdoc />
    public async Task<TenantExecutionParameter> UpdateTenantExecutionParameterAsync(int tenantId, int executionParameterId, TenantExecutionParameterUpsertModel model, CancellationToken cancellationToken)
    {
        ValidateTenantExecutionParameter(model);
        var parameter = await administrationDA.GetTenantExecutionParameterAsync(tenantId, executionParameterId, cancellationToken)
            ?? throw new KeyNotFoundException($"Tenant execution parameter {executionParameterId} was not found for tenant {tenantId}.");

        parameter.Key = model.Key.Trim();
        parameter.Value = model.Value;
        parameter.UpdatedBy = NormalizeOptional(model.UpdatedBy);
        parameter.UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime;
        await administrationDA.SaveChangesAsync(cancellationToken);
        return parameter;
    }

    /// <inheritdoc />
    public Task<PagedResult<BatchSchedule>> GetBatchSchedulesAsync(int batchJobId, int page, int pageSize, CancellationToken cancellationToken) =>
        administrationDA.GetBatchSchedulesAsync(batchJobId, NormalizePage(page), NormalizePageSize(pageSize), cancellationToken);

    /// <inheritdoc />
    public async Task<BatchSchedule> CreateBatchScheduleAsync(int batchJobId, BatchScheduleUpsertModel model, CancellationToken cancellationToken)
    {
        _ = await administrationDA.GetBatchJobAsync(batchJobId, cancellationToken) ?? throw new KeyNotFoundException($"Batch job {batchJobId} was not found.");
        var schedule = await CreateScheduleAsync(batchJobId, model, cancellationToken);
        await administrationDA.AddBatchScheduleAsync(schedule, cancellationToken);
        return schedule;
    }

    /// <inheritdoc />
    public async Task<BatchSchedule> UpdateBatchScheduleAsync(int batchScheduleId, BatchScheduleUpsertModel model, CancellationToken cancellationToken)
    {
        var schedule = await administrationDA.GetBatchScheduleAsync(batchScheduleId, cancellationToken)
            ?? throw new KeyNotFoundException($"Batch schedule {batchScheduleId} was not found.");

        ApplySchedule(schedule, model);
        await ValidateScheduleAsync(schedule, cancellationToken);
        schedule.NextFireUtc = await scheduleCalculator.GetNextOccurrenceUtcAsync(schedule, timeProvider.GetUtcNow().UtcDateTime.AddSeconds(-1), cancellationToken);
        schedule.UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime;
        await administrationDA.SaveChangesAsync(cancellationToken);
        return schedule;
    }

    /// <inheritdoc />
    public Task<PagedResult<TenantJobScheduleBinding>> GetScheduleBindingsAsync(int tenantId, int batchJobId, int page, int pageSize, CancellationToken cancellationToken) =>
        administrationDA.GetScheduleBindingsAsync(tenantId, batchJobId, NormalizePage(page), NormalizePageSize(pageSize), cancellationToken);

    /// <inheritdoc />
    public async Task<TenantJobScheduleBinding> CreateScheduleBindingAsync(int tenantId, int batchJobId, TenantJobScheduleBindingUpsertModel model, CancellationToken cancellationToken)
    {
        ValidateScheduleBinding(model);
        var tenantJob = await administrationDA.GetTenantJobAsync(tenantId, batchJobId, cancellationToken)
            ?? throw new ValidationException($"Tenant {tenantId} must have a tenant-job configuration for job {batchJobId} before a schedule binding can be created.");
        var schedule = await administrationDA.GetBatchScheduleAsync(model.BatchScheduleId, cancellationToken)
            ?? throw new KeyNotFoundException($"Batch schedule {model.BatchScheduleId} was not found.");
        if (schedule.BatchJobId != batchJobId || tenantJob.BatchJobId != batchJobId)
        {
            throw new ValidationException("Schedule bindings must reference a schedule for the same batch job as the tenant-job configuration.");
        }

        var binding = new TenantJobScheduleBinding
        {
            TenantId = tenantId,
            BatchJobId = batchJobId,
            BatchScheduleId = model.BatchScheduleId,
            IsEnabled = model.IsEnabled,
            EffectiveFromUtc = model.EffectiveFromUtc,
            EffectiveToUtc = model.EffectiveToUtc,
            UpdatedBy = NormalizeOptional(model.UpdatedBy),
            UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime,
        };

        await administrationDA.AddScheduleBindingAsync(binding, cancellationToken);
        return binding;
    }

    private static void ValidateTenant(TenantUpsertModel model)
    {
        Require(model.Code, "Tenant code is required.");
        Require(model.Name, "Tenant name is required.");
        Require(model.TimeZone, "Tenant time zone is required.");
        _ = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZone.Trim());
    }

    private static void ValidateRunnerProfile(RunnerProfileUpsertModel model)
    {
        Require(model.Name, "Runner profile name is required.");
        Require(model.ExecutableTemplate, "Runner executable template is required.");
        if (model.RunnerType != BajRunnerType.Process)
        {
            throw new ValidationException("RUN_RUNNER_TYPE must be Process in v1.");
        }

        if (model.ProfileMode == RunProfileMode.CustomTemplate && string.IsNullOrWhiteSpace(model.ArgumentTemplate))
        {
            throw new ValidationException("RUN_ARGUMENT_TEMPLATE is required when RUN_PROFILE_MODE = CustomTemplate.");
        }
    }

    private async Task ValidateBatchJobAsync(BatchJobUpsertModel model, CancellationToken cancellationToken)
    {
        Require(model.JobName, "Batch job name is required.");
        if (model.RunnerType != BajRunnerType.Process)
        {
            throw new ValidationException("BAJ_RUNNER_TYPE must be Process in v1.");
        }

        var runnerProfile = await administrationDA.GetRunnerProfileAsync(model.RunnerProfileId, cancellationToken)
            ?? throw new ValidationException($"Runner profile {model.RunnerProfileId} was not found.");
        if (!runnerProfile.IsEnabled)
        {
            throw new ValidationException("BAJ_RUN_ID must reference an enabled runner profile.");
        }

        if (model.TimeoutSeconds < 0)
        {
            throw new ValidationException("BAJ_TIMEOUT_SEC must be greater than or equal to 0.");
        }

        if (model.MaxParallelGlobal is <= 0)
        {
            throw new ValidationException("BAJ_MAX_PARALLEL_GLOBAL must be greater than or equal to 1 when provided.");
        }

        if (model.MaxParallelPerTenant is <= 0)
        {
            throw new ValidationException("BAJ_MAX_PARALLEL_PER_TENANT must be greater than or equal to 1 when provided.");
        }

        ValidateDateArgument(model);
    }

    private static void ValidateDateArgument(BatchJobUpsertModel model)
    {
        if (model.DateArgumentMode == BajDateArgumentMode.None)
        {
            return;
        }

        Require(model.DateFormat, "BAJ_DATE_FORMAT is required when a date argument mode is configured.");
        if (model.DateArgumentMode is BajDateArgumentMode.NamedValueSameToken or BajDateArgumentMode.NamedSeparateToken)
        {
            Require(model.DateArgumentName, "BAJ_DATE_ARGUMENT_NAME is required for named date argument modes.");
        }
    }

    private static void ValidateTenantJob(TenantJobUpsertModel model)
    {
        if (!string.IsNullOrWhiteSpace(model.ParameterJson))
        {
            JsonDocument.Parse(model.ParameterJson);
        }
    }

    private static void ValidateTenantExecutionParameter(TenantExecutionParameterUpsertModel model)
    {
        Require(model.Key, "Tenant execution parameter key is required.");
        if (model.Value is null)
        {
            throw new ValidationException("Tenant execution parameter value is required.");
        }
    }

    private async Task<BatchSchedule> CreateScheduleAsync(int batchJobId, BatchScheduleUpsertModel model, CancellationToken cancellationToken)
    {
        var schedule = new BatchSchedule { BatchJobId = batchJobId };
        ApplySchedule(schedule, model);
        await ValidateScheduleAsync(schedule, cancellationToken);
        var now = timeProvider.GetUtcNow().UtcDateTime;
        schedule.NextFireUtc = await scheduleCalculator.GetNextOccurrenceUtcAsync(schedule, now.AddSeconds(-1), cancellationToken);
        schedule.UpdatedUtc = now;
        return schedule;
    }

    private async Task ValidateScheduleAsync(BatchSchedule schedule, CancellationToken cancellationToken)
    {
        if (schedule.RecheckEverySeconds is <= 0)
        {
            throw new ValidationException("BSC_RECHECK_EVERY_SEC must be greater than or equal to 1 when provided.");
        }

        if (schedule.MaxParallelGroup is <= 0)
        {
            throw new ValidationException("BSC_MAX_PARALLEL_GROUP must be greater than or equal to 1 when provided.");
        }

        Require(schedule.Name, "Batch schedule name is required.");
        Require(schedule.TimeZone, "Batch schedule time zone is required.");
        Require(schedule.ConfigJson, "Batch schedule configuration JSON is required.");
        await scheduleCalculator.ValidateAsync(schedule, cancellationToken);
    }

    private static void ValidateScheduleBinding(TenantJobScheduleBindingUpsertModel model)
    {
        if (model.EffectiveFromUtc.HasValue && model.EffectiveToUtc.HasValue && model.EffectiveFromUtc > model.EffectiveToUtc)
        {
            throw new ValidationException("Schedule binding effectiveFromUtc must be earlier than or equal to effectiveToUtc.");
        }
    }

    private static BatchJob CreateBatchJob(BatchJobUpsertModel model, DateTime now)
    {
        var batchJob = new BatchJob { CreatedUtc = now, UpdatedUtc = now };
        ApplyBatchJob(batchJob, model);
        return batchJob;
    }

    private static void ApplyBatchJob(BatchJob batchJob, BatchJobUpsertModel model)
    {
        batchJob.JobName = model.JobName.Trim();
        batchJob.Description = NormalizeOptional(model.Description);
        batchJob.RunnerType = model.RunnerType;
        batchJob.RunnerProfileId = model.RunnerProfileId;
        batchJob.CommandTarget = NormalizeOptional(model.CommandTarget);
        batchJob.ExtraArgumentsTemplate = NormalizeOptional(model.ExtraArgumentsTemplate);
        batchJob.DateArgumentMode = model.DateArgumentMode;
        batchJob.DateArgumentName = NormalizeOptional(model.DateArgumentName);
        batchJob.DateFormat = NormalizeOptional(model.DateFormat);
        batchJob.DateOffsetDays = model.DateArgumentMode == BajDateArgumentMode.None ? null : model.DateOffsetDays ?? 0;
        batchJob.WorkDirectoryOverride = NormalizeOptional(model.WorkDirectoryOverride);
        batchJob.TimeoutSeconds = model.TimeoutSeconds;
        batchJob.MutexGroup = NormalizeOptional(model.MutexGroup);
        batchJob.MaxParallelGlobal = model.MaxParallelGlobal;
        batchJob.MaxParallelPerTenant = model.MaxParallelPerTenant;
        batchJob.EnabledDefault = model.EnabledDefault;
    }

    private static void ApplySchedule(BatchSchedule schedule, BatchScheduleUpsertModel model)
    {
        schedule.Name = model.Name.Trim();
        schedule.ScheduleType = model.ScheduleType;
        schedule.TimeZone = model.TimeZone.Trim();
        schedule.ConfigJson = model.ConfigJson.Trim();
        schedule.RecheckEverySeconds = model.RecheckEverySeconds;
        schedule.LatestStartLocal = model.LatestStartLocal;
        schedule.MisfirePolicy = model.MisfirePolicy;
        schedule.IsEnabled = model.IsEnabled;
        schedule.MaxParallelGroup = model.MaxParallelGroup;
        schedule.UpdatedBy = NormalizeOptional(model.UpdatedBy);
    }

    private static void Require(string? value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ValidationException(message);
        }
    }

    private static string? NormalizeOptional(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static int NormalizePage(int page) => page < 1 ? 1 : page;

    private static int NormalizePageSize(int pageSize) => pageSize switch
    {
        <= 0 => 50,
        > 200 => 200,
        _ => pageSize,
    };
}
