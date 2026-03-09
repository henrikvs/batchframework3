namespace Batch.Domain.Entities;

/// <summary>
/// Stores holiday calendar data used by bank-day schedule calculations.
/// </summary>
public sealed class HolidayCalendarEntry
{
    /// <summary>
    /// Gets or sets the internal holiday identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the calendar code used for holiday grouping.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the holiday date.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the human-readable holiday name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC timestamp when the holiday row was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; }
}
