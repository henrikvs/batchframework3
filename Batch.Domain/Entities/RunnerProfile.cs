using Batch.Domain.Enums;

namespace Batch.Domain.Entities;

/// <summary>
/// Defines reusable process-launch behavior shared by multiple jobs.
/// </summary>
public sealed class RunnerProfile
{
    /// <summary>
    /// Gets or sets the internal runner profile identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the stable administrative name of the runner profile.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional description of the runner profile.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the high-level runner type.
    /// </summary>
    public BajRunnerType RunnerType { get; set; }

    /// <summary>
    /// Gets or sets the reusable launch behavior mode.
    /// </summary>
    public RunProfileMode ProfileMode { get; set; }

    /// <summary>
    /// Gets or sets the executable path or template used by the profile.
    /// </summary>
    public string ExecutableTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional working directory template used by the profile.
    /// </summary>
    public string? WorkDirectoryTemplate { get; set; }

    /// <summary>
    /// Gets or sets the optional base argument template.
    /// </summary>
    public string? ArgumentTemplate { get; set; }

    /// <summary>
    /// Gets or sets the optional token joiner used for template-generated arguments.
    /// </summary>
    public string? ArgumentJoiner { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the profile may be used by active jobs.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the profile was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the profile was last updated.
    /// </summary>
    public DateTime UpdatedUtc { get; set; }
}
