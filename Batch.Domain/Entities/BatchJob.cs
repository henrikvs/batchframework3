using Batch.Domain.Enums;

namespace Batch.Domain.Entities;

/// <summary>
/// Defines the global job identity and execution behavior shared by all tenants.
/// </summary>
public sealed class BatchJob
{
    /// <summary>
    /// Gets or sets the internal batch job identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the stable logical job name.
    /// </summary>
    public string JobName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional human-readable job description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the high-level runner type.
    /// </summary>
    public BajRunnerType RunnerType { get; set; }

    /// <summary>
    /// Gets or sets the runner profile identifier used to launch the job.
    /// </summary>
    public int RunnerProfileId { get; set; }

    /// <summary>
    /// Gets or sets the optional job-specific command target token.
    /// </summary>
    public string? CommandTarget { get; set; }

    /// <summary>
    /// Gets or sets the optional extra arguments appended after the base launch arguments.
    /// </summary>
    public string? ExtraArgumentsTemplate { get; set; }

    /// <summary>
    /// Gets or sets the date argument rendering mode.
    /// </summary>
    public BajDateArgumentMode DateArgumentMode { get; set; }

    /// <summary>
    /// Gets or sets the optional date argument name.
    /// </summary>
    public string? DateArgumentName { get; set; }

    /// <summary>
    /// Gets or sets the optional .NET date format string.
    /// </summary>
    public string? DateFormat { get; set; }

    /// <summary>
    /// Gets or sets the optional date offset in days.
    /// </summary>
    public int? DateOffsetDays { get; set; }

    /// <summary>
    /// Gets or sets the optional per-job working directory override.
    /// </summary>
    public string? WorkDirectoryOverride { get; set; }

    /// <summary>
    /// Gets or sets the maximum execution duration in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; }

    /// <summary>
    /// Gets or sets the optional tenant-level mutex group name.
    /// </summary>
    public string? MutexGroup { get; set; }

    /// <summary>
    /// Gets or sets the optional global concurrency cap.
    /// </summary>
    public int? MaxParallelGlobal { get; set; }

    /// <summary>
    /// Gets or sets the optional per-tenant concurrency cap.
    /// </summary>
    public int? MaxParallelPerTenant { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether newly onboarded tenants should default to enabled.
    /// </summary>
    public bool EnabledDefault { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the job was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the job was last updated.
    /// </summary>
    public DateTime UpdatedUtc { get; set; }
}
