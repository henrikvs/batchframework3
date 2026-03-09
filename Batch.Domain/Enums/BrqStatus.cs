namespace Batch.Domain.Enums;

/// <summary>
/// Represents the lifecycle state of a run request.
/// </summary>
public enum BrqStatus
{
    /// <summary>
    /// The request exists but has not yet been fired into instances.
    /// </summary>
    Planned,

    /// <summary>
    /// The request has been expanded into instances.
    /// </summary>
    Fired,

    /// <summary>
    /// The request was cancelled before firing.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The request failed during orchestration.
    /// </summary>
    Failed,
}
