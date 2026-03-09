using Batch.Domain.Enums;

namespace Batch.Domain.Entities;

/// <summary>
/// Defines a reusable recurring schedule for a specific job.
/// </summary>
public sealed class BatchSchedule
{
    /// <summary>
    /// Gets or sets the internal batch schedule identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the batch job identifier.
    /// </summary>
    public int BatchJobId { get; set; }

    /// <summary>
    /// Gets or sets the human-readable schedule name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the schedule calculation strategy.
    /// </summary>
    public BscScheduleType ScheduleType { get; set; }

    /// <summary>
    /// Gets or sets the time zone in which the schedule is evaluated.
    /// </summary>
    public string TimeZone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the JSON schedule configuration payload.
    /// </summary>
    public string ConfigJson { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional blocked-instance reevaluation cadence in seconds.
    /// </summary>
    public int? RecheckEverySeconds { get; set; }

    /// <summary>
    /// Gets or sets the optional local cutoff time after which a blocked occurrence should not start.
    /// </summary>
    public TimeOnly? LatestStartLocal { get; set; }

    /// <summary>
    /// Gets or sets the misfire handling policy.
    /// </summary>
    public BscMisfirePolicy MisfirePolicy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the schedule participates in the scheduler loop.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the optional concurrency cap for instances originating from the schedule.
    /// </summary>
    public int? MaxParallelGroup { get; set; }

    /// <summary>
    /// Gets or sets the next UTC time the scheduler should materialize into a run request.
    /// </summary>
    public DateTime? NextFireUtc { get; set; }

    /// <summary>
    /// Gets or sets the last generated slot key.
    /// </summary>
    public string? LastSlotKey { get; set; }

    /// <summary>
    /// Gets or sets the optional user or system responsible for the latest schedule update.
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the schedule was last updated.
    /// </summary>
    public DateTime UpdatedUtc { get; set; }
}
