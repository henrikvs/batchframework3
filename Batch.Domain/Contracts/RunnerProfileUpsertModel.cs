using Batch.Domain.Enums;

namespace Batch.Domain.Contracts;

/// <summary>
/// Represents the mutable runner profile fields accepted by create and update operations.
/// </summary>
public sealed class RunnerProfileUpsertModel
{
    /// <summary>
    /// Gets or sets the stable administrative runner profile name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional runner profile description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the high-level runner type.
    /// </summary>
    public BajRunnerType RunnerType { get; set; }

    /// <summary>
    /// Gets or sets the reusable process launch mode.
    /// </summary>
    public RunProfileMode ProfileMode { get; set; }

    /// <summary>
    /// Gets or sets the executable path or template.
    /// </summary>
    public string ExecutableTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional working directory template.
    /// </summary>
    public string? WorkDirectoryTemplate { get; set; }

    /// <summary>
    /// Gets or sets the optional base argument template.
    /// </summary>
    public string? ArgumentTemplate { get; set; }

    /// <summary>
    /// Gets or sets the optional argument joiner.
    /// </summary>
    public string? ArgumentJoiner { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the profile may be used by active jobs.
    /// </summary>
    public bool IsEnabled { get; set; }
}
