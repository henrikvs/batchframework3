namespace Batch.Domain.Enums;

/// <summary>
/// Represents how a run request was created.
/// </summary>
public enum BrqTriggerType
{
    /// <summary>
    /// The request was created by the scheduler.
    /// </summary>
    Schedule,

    /// <summary>
    /// The request was created manually.
    /// </summary>
    Manual,

    /// <summary>
    /// The request was created through the API.
    /// </summary>
    Api,

    /// <summary>
    /// The request was created from dependency expansion.
    /// </summary>
    Dependency,
}
