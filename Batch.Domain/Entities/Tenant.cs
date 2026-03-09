using Batch.Domain.Enums;

namespace Batch.Domain.Entities;

/// <summary>
/// Stores the master list of tenants that may participate in scheduling and execution.
/// </summary>
public sealed class Tenant
{
    /// <summary>
    /// Gets or sets the internal tenant identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the stable business code used to identify the tenant.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the human-readable tenant name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tenant lifecycle status.
    /// </summary>
    public TenStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the default time zone identifier for the tenant.
    /// </summary>
    public string TimeZone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional canonical root path used for CLI pre-flight resolution.
    /// </summary>
    public string? RootPath { get; set; }

    /// <summary>
    /// Gets or sets the optional free-form tag list.
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the tenant was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the tenant was last updated.
    /// </summary>
    public DateTime UpdatedUtc { get; set; }
}
