namespace Batch.Domain.Contracts;

/// <summary>
/// Represents the mutable tenant execution parameter fields accepted by create and update operations.
/// </summary>
public sealed class TenantExecutionParameterUpsertModel
{
    /// <summary>
    /// Gets or sets the stable parameter key.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the opaque parameter value.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional user or system responsible for the update.
    /// </summary>
    public string? UpdatedBy { get; set; }
}
