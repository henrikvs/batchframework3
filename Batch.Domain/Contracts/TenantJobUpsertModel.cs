namespace Batch.Domain.Contracts;

/// <summary>
/// Represents the mutable tenant-job fields accepted by create and update operations.
/// </summary>
public sealed class TenantJobUpsertModel
{
    /// <summary>
    /// Gets or sets a value indicating whether the tenant may execute the job.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the optional tenant-level default parameter payload.
    /// </summary>
    public string? ParameterJson { get; set; }

    /// <summary>
    /// Gets or sets the optional tenant-specific priority override.
    /// </summary>
    public short? Priority { get; set; }

    /// <summary>
    /// Gets or sets the optional user or system responsible for the update.
    /// </summary>
    public string? UpdatedBy { get; set; }
}
