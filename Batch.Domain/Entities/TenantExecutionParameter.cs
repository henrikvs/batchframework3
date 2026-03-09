namespace Batch.Domain.Entities;

/// <summary>
/// Stores generic tenant-specific execution parameters required by runner profiles or templates.
/// </summary>
public sealed class TenantExecutionParameter
{
    /// <summary>
    /// Gets or sets the internal tenant execution parameter identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the tenant identifier that owns the parameter.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Gets or sets the stable parameter key.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the opaque parameter value.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional user or system responsible for the latest update.
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the parameter was last updated.
    /// </summary>
    public DateTime UpdatedUtc { get; set; }
}
