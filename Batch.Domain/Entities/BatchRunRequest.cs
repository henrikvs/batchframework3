using Batch.Domain.Enums;

namespace Batch.Domain.Entities;

/// <summary>
/// Represents an intent to execute work in the future or immediately.
/// </summary>
public sealed class BatchRunRequest
{
    /// <summary>
    /// Gets or sets the internal run request identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets how the request was created.
    /// </summary>
    public BrqTriggerType TriggerType { get; set; }

    /// <summary>
    /// Gets or sets the optional direct target tenant identifier.
    /// </summary>
    public int? TenantId { get; set; }

    /// <summary>
    /// Gets or sets the optional direct target batch job identifier.
    /// </summary>
    public int? BatchJobId { get; set; }

    /// <summary>
    /// Gets or sets the optional originating batch schedule identifier.
    /// </summary>
    public int? BatchScheduleId { get; set; }

    /// <summary>
    /// Gets or sets the optional deterministic schedule occurrence key.
    /// </summary>
    public string? SlotKey { get; set; }

    /// <summary>
    /// Gets or sets the UTC time when the request becomes eligible to fire.
    /// </summary>
    public DateTime FireAtUtc { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether prerequisite jobs should also be materialized.
    /// </summary>
    public bool IncludePrerequisites { get; set; }

    /// <summary>
    /// Gets or sets how the request should expand into instances.
    /// </summary>
    public BrqMode Mode { get; set; }

    /// <summary>
    /// Gets or sets the optional multi-tenant target selector payload.
    /// </summary>
    public string? TargetTenantFilterJson { get; set; }

    /// <summary>
    /// Gets or sets the optional structured execution context payload.
    /// </summary>
    public string? ExecutionContextJson { get; set; }

    /// <summary>
    /// Gets or sets the canonical orchestration scope key.
    /// </summary>
    public string ScopeKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether dependency checks should be enforced.
    /// </summary>
    public bool RespectDependencies { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gate checks should be enforced.
    /// </summary>
    public bool RespectGates { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the request should do nothing if not immediately ready.
    /// </summary>
    public bool OnlyIfReady { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gate checks may be bypassed.
    /// </summary>
    public bool ForceGates { get; set; }

    /// <summary>
    /// Gets or sets the request lifecycle state.
    /// </summary>
    public BrqStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the optional human-readable reason or note.
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Gets or sets the user, CLI, or system name that created the request.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC timestamp when the request was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the request was last updated.
    /// </summary>
    public DateTime UpdatedUtc { get; set; }
}
