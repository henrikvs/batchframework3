using Batch.Domain.Enums;

namespace Batch.Domain.Contracts;

/// <summary>
/// Represents the mutable batch job fields accepted by create and update operations.
/// </summary>
public sealed class BatchJobUpsertModel
{
    /// <summary>
    /// Gets or sets the stable logical job name.
    /// </summary>
    public string JobName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional job description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the high-level runner type.
    /// </summary>
    public BajRunnerType RunnerType { get; set; }

    /// <summary>
    /// Gets or sets the runner profile identifier.
    /// </summary>
    public int RunnerProfileId { get; set; }

    /// <summary>
    /// Gets or sets the optional command target token.
    /// </summary>
    public string? CommandTarget { get; set; }

    /// <summary>
    /// Gets or sets the optional extra argument template.
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
    /// Gets or sets the optional working directory override.
    /// </summary>
    public string? WorkDirectoryOverride { get; set; }

    /// <summary>
    /// Gets or sets the timeout in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; }

    /// <summary>
    /// Gets or sets the optional mutex group.
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
    /// Gets or sets a value indicating whether new tenant-job rows should default to enabled.
    /// </summary>
    public bool EnabledDefault { get; set; }
}
