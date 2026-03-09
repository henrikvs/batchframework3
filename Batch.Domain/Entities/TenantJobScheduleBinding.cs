namespace Batch.Domain.Entities;

/// <summary>
/// Binds a tenant job to a reusable batch schedule.
/// </summary>
public sealed class TenantJobScheduleBinding
{
    /// <summary>
    /// Gets or sets the internal binding identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the tenant identifier.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Gets or sets the batch job identifier.
    /// </summary>
    public int BatchJobId { get; set; }

    /// <summary>
    /// Gets or sets the batch schedule identifier.
    /// </summary>
    public int BatchScheduleId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the binding is active.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the optional lower activation bound.
    /// </summary>
    public DateTime? EffectiveFromUtc { get; set; }

    /// <summary>
    /// Gets or sets the optional upper activation bound.
    /// </summary>
    public DateTime? EffectiveToUtc { get; set; }

    /// <summary>
    /// Gets or sets the optional user or system responsible for the latest binding change.
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the binding was last updated.
    /// </summary>
    public DateTime UpdatedUtc { get; set; }
}
