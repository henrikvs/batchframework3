using Batch.Domain.Enums;

namespace Batch.Domain.Contracts;

/// <summary>
/// Represents the mutable tenant fields accepted by create and update operations.
/// </summary>
public sealed class TenantUpsertModel
{
    /// <summary>
    /// Gets or sets the stable tenant business code.
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
    /// Gets or sets the default tenant time zone identifier.
    /// </summary>
    public string TimeZone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional canonical tenant root path.
    /// </summary>
    public string? RootPath { get; set; }

    /// <summary>
    /// Gets or sets the optional free-form tenant tags.
    /// </summary>
    public string? Tags { get; set; }
}
