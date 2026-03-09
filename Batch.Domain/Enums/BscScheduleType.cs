namespace Batch.Domain.Enums;

/// <summary>
/// Represents the supported recurring schedule calculation strategies.
/// </summary>
public enum BscScheduleType
{
    /// <summary>
    /// Runs once each day at a configured local time.
    /// </summary>
    DailyAt,

    /// <summary>
    /// Runs weekly on specific weekdays at a configured local time.
    /// </summary>
    WeeklyAt,

    /// <summary>
    /// Runs repeatedly inside a configured daily time window.
    /// </summary>
    RepeatInWindow,

    /// <summary>
    /// Runs on the last bank day of the month at a configured local time.
    /// </summary>
    LastBankDay,

    /// <summary>
    /// Runs multiple configured times per day.
    /// </summary>
    MultipleTimesPerDay,
}
