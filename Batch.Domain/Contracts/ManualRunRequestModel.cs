namespace Batch.Domain.Contracts;

/// <summary>
/// Represents the API contract for creating a manual single-tenant run request.
/// </summary>
public sealed class ManualRunRequestModel
{
    /// <summary>
    /// Gets or sets the target batch job identifier.
    /// </summary>
    public int BatchJobId { get; set; }

    /// <summary>
    /// Gets or sets the optional orchestration scope key. When omitted, an empty string is used.
    /// </summary>
    public string? ScopeKey { get; set; }

    /// <summary>
    /// Gets or sets the optional structured execution context payload.
    /// </summary>
    public string? ExecutionContextJson { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether prerequisite jobs should be included.
    /// </summary>
    public bool IncludePrerequisites { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gate checks may be bypassed.
    /// </summary>
    public bool ForceGates { get; set; }

    /// <summary>
    /// Gets or sets the user or system name that created the request.
    /// </summary>
    public string? CreatedBy { get; set; }
}
