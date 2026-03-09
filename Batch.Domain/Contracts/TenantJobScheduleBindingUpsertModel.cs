namespace Batch.Domain.Contracts;

/// <summary>
/// Represents the mutable schedule-binding fields accepted by create operations.
/// </summary>
public sealed class TenantJobScheduleBindingUpsertModel
{
    /// <summary>
    /// Gets or sets the batch schedule identifier.
    /// </summary>
    public int BatchScheduleId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the binding is active.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the optional lower activation bound in UTC.
    /// </summary>
    public DateTime? EffectiveFromUtc { get; set; }

    /// <summary>
    /// Gets or sets the optional upper activation bound in UTC.
    /// </summary>
    public DateTime? EffectiveToUtc { get; set; }

    /// <summary>
    /// Gets or sets the optional user or system responsible for the update.
    /// </summary>
    public string? UpdatedBy { get; set; }
}
