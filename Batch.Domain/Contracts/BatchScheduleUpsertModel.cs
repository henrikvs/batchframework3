using Batch.Domain.Enums;

namespace Batch.Domain.Contracts;

/// <summary>
/// Represents the mutable batch schedule fields accepted by create and update operations.
/// </summary>
public sealed class BatchScheduleUpsertModel
{
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
    /// Gets or sets the optional reevaluation cadence in seconds.
    /// </summary>
    public int? RecheckEverySeconds { get; set; }

    /// <summary>
    /// Gets or sets the optional local cutoff time.
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
    /// Gets or sets the optional schedule-level concurrency cap.
    /// </summary>
    public int? MaxParallelGroup { get; set; }

    /// <summary>
    /// Gets or sets the optional user or system responsible for the update.
    /// </summary>
    public string? UpdatedBy { get; set; }
}
