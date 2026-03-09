using Batch.Domain.Enums;

namespace Batch.Domain.Entities;

/// <summary>
/// Represents actual runnable work for a specific job, tenant, occurrence, and scope.
/// </summary>
public sealed class BatchJobInstance
{
    /// <summary>
    /// Gets or sets the internal batch job instance identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the run request identifier that created the instance.
    /// </summary>
    public Guid BatchRunRequestId { get; set; }

    /// <summary>
    /// Gets or sets the target tenant identifier.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Gets or sets the target batch job identifier.
    /// </summary>
    public int BatchJobId { get; set; }

    /// <summary>
    /// Gets or sets the optional originating schedule identifier.
    /// </summary>
    public int? BatchScheduleId { get; set; }

    /// <summary>
    /// Gets or sets the occurrence key identifying the logical occurrence.
    /// </summary>
    public string OccurrenceKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the canonical orchestration scope key.
    /// </summary>
    public string ScopeKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional execution context payload resolved for the runner.
    /// </summary>
    public string? ExecutionContextJson { get; set; }

    /// <summary>
    /// Gets or sets the logical planned start time in UTC.
    /// </summary>
    public DateTime PlannedStartUtc { get; set; }

    /// <summary>
    /// Gets or sets the current lifecycle state.
    /// </summary>
    public BinStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the optional human-readable block or failure reason.
    /// </summary>
    public string? BlockReason { get; set; }

    /// <summary>
    /// Gets or sets the latest attempt number for the instance.
    /// </summary>
    public int AttemptNumber { get; set; }

    /// <summary>
    /// Gets or sets the earliest time the evaluator should reconsider the instance.
    /// </summary>
    public DateTime? NextEvaluationUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the instance was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the instance was last updated.
    /// </summary>
    public DateTime UpdatedUtc { get; set; }
}
