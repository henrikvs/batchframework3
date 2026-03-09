namespace Batch.Domain.Enums;

/// <summary>
/// Represents the lifecycle state of a job instance.
/// </summary>
public enum BinStatus
{
    /// <summary>
    /// The instance has been created but not yet evaluated.
    /// </summary>
    Planned,

    /// <summary>
    /// The instance is blocked and awaiting reevaluation.
    /// </summary>
    Blocked,

    /// <summary>
    /// The instance is ready for dispatch.
    /// </summary>
    Ready,

    /// <summary>
    /// The instance is currently running.
    /// </summary>
    Running,

    /// <summary>
    /// The instance completed successfully.
    /// </summary>
    Succeeded,

    /// <summary>
    /// The instance failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The instance was skipped.
    /// </summary>
    Skipped,

    /// <summary>
    /// The instance was cancelled.
    /// </summary>
    Cancelled,
}
