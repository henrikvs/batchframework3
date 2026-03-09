namespace Batch.Domain.Enums;

/// <summary>
/// Represents how missed schedule occurrences are handled.
/// </summary>
public enum BscMisfirePolicy
{
    /// <summary>
    /// Skips missed occurrences and advances to the next future slot.
    /// </summary>
    Skip,

    /// <summary>
    /// Collapses missed occurrences into one immediate catch-up request.
    /// </summary>
    RunOnceNextWindow,
}
