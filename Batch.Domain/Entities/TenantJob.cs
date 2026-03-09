namespace Batch.Domain.Entities;

/// <summary>
/// Stores tenant-specific configuration for a global job.
/// </summary>
public sealed class TenantJob
{
    /// <summary>
    /// Gets or sets the internal tenant-job identifier.
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
    /// Gets or sets the optional user or system responsible for the latest change.
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the tenant-job configuration was last updated.
    /// </summary>
    public DateTime UpdatedUtc { get; set; }
}
