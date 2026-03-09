using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Batch.DataAccess;
using Batch.Domain.Entities;
using Batch.Domain.Enums;

namespace Batch.Services.Scheduling;

/// <summary>
/// Validates schedule configuration payloads and computes future occurrences.
/// </summary>
public sealed class ScheduleCalculator(IAdministrationDA administrationDA)
{
    /// <summary>
    /// Validates that the schedule configuration matches the declared schedule type.
    /// </summary>
    public async Task ValidateAsync(BatchSchedule schedule, CancellationToken cancellationToken)
    {
        _ = FindTimeZone(schedule.TimeZone);

        using var document = JsonDocument.Parse(schedule.ConfigJson);
        var root = document.RootElement;

        switch (schedule.ScheduleType)
        {
            case BscScheduleType.DailyAt:
                _ = ReadTime(root, "localTime");
                break;
            case BscScheduleType.WeeklyAt:
                _ = ReadTime(root, "localTime");
                var days = ReadDaysOfWeek(root, "daysOfWeek");
                if (days.Count == 0)
                {
                    throw new ValidationException("Schedule configuration must contain at least one dayOfWeek value.");
                }

                break;
            case BscScheduleType.RepeatInWindow:
                var windowStart = ReadTime(root, "windowStart");
                var windowEnd = ReadTime(root, "windowEnd");
                var intervalMinutes = ReadRequiredInt(root, "intervalMinutes");
                if (windowStart >= windowEnd)
                {
                    throw new ValidationException("RepeatInWindow requires windowStart to be earlier than windowEnd.");
                }

                if (intervalMinutes < 1)
                {
                    throw new ValidationException("RepeatInWindow requires intervalMinutes to be at least 1.");
                }

                break;
            case BscScheduleType.LastBankDay:
                _ = ReadTime(root, "localTime");
                var calendarCode = ReadRequiredString(root, "calendarCode");
                _ = await administrationDA.GetHolidayCalendarEntriesAsync(calendarCode, DateOnly.MinValue, DateOnly.MinValue, cancellationToken);
                break;
            case BscScheduleType.MultipleTimesPerDay:
                var times = ReadTimes(root, "times");
                if (times.Count == 0)
                {
                    throw new ValidationException("MultipleTimesPerDay requires at least one time value.");
                }

                if (times.Count != times.Distinct().Count())
                {
                    throw new ValidationException("MultipleTimesPerDay requires unique time values.");
                }

                break;
            default:
                throw new ValidationException($"Unsupported schedule type '{schedule.ScheduleType}'.");
        }
    }

    /// <summary>
    /// Computes the next UTC occurrence strictly after the supplied UTC instant.
    /// </summary>
    public async Task<DateTime?> GetNextOccurrenceUtcAsync(BatchSchedule schedule, DateTime utcAfterExclusive, CancellationToken cancellationToken)
    {
        var timeZone = FindTimeZone(schedule.TimeZone);
        using var document = JsonDocument.Parse(schedule.ConfigJson);
        var root = document.RootElement;

        return schedule.ScheduleType switch
        {
            BscScheduleType.DailyAt => GetNextDailyOccurrence(timeZone, utcAfterExclusive, ReadTime(root, "localTime")),
            BscScheduleType.WeeklyAt => GetNextWeeklyOccurrence(timeZone, utcAfterExclusive, ReadTime(root, "localTime"), ReadDaysOfWeek(root, "daysOfWeek")),
            BscScheduleType.RepeatInWindow => GetNextRepeatInWindowOccurrence(
                timeZone,
                utcAfterExclusive,
                ReadTime(root, "windowStart"),
                ReadTime(root, "windowEnd"),
                ReadRequiredInt(root, "intervalMinutes")),
            BscScheduleType.LastBankDay => await GetNextLastBankDayOccurrenceAsync(
                timeZone,
                utcAfterExclusive,
                ReadTime(root, "localTime"),
                ReadRequiredString(root, "calendarCode"),
                cancellationToken),
            BscScheduleType.MultipleTimesPerDay => GetNextMultipleTimesOccurrence(timeZone, utcAfterExclusive, ReadTimes(root, "times")),
            _ => null,
        };
    }

    /// <summary>
    /// Builds the deterministic slot key used for schedule-created run requests.
    /// </summary>
    public string BuildSlotKey(BatchSchedule schedule, DateTime fireAtUtc)
    {
        var timeZone = FindTimeZone(schedule.TimeZone);
        var localSlot = TimeZoneInfo.ConvertTimeFromUtc(fireAtUtc, timeZone);
        return $"{fireAtUtc:yyyy-MM-ddTHH:mm:ss}|{timeZone.Id}|{schedule.ScheduleType}|{localSlot:yyyy-MM-ddTHH:mm:ss}";
    }

    private static TimeZoneInfo FindTimeZone(string timeZoneId)
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch (TimeZoneNotFoundException ex)
        {
            _ = ex;
            throw new ValidationException($"Time zone '{timeZoneId}' was not found.");
        }
        catch (InvalidTimeZoneException ex)
        {
            _ = ex;
            throw new ValidationException($"Time zone '{timeZoneId}' is invalid.");
        }
    }

    private static TimeOnly ReadTime(JsonElement root, string propertyName)
    {
        var value = ReadRequiredString(root, propertyName);
        if (!TimeOnly.TryParseExact(value, "HH:mm:ss", out var result))
        {
            throw new ValidationException($"Schedule configuration field '{propertyName}' must use HH:mm:ss format.");
        }

        return result;
    }

    private static IReadOnlyList<TimeOnly> ReadTimes(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var value) || value.ValueKind != JsonValueKind.Array)
        {
            throw new ValidationException($"Schedule configuration field '{propertyName}' must be an array.");
        }

        return value.EnumerateArray().Select(x => ReadTimeValue(propertyName, x)).OrderBy(x => x).ToArray();
    }

    private static TimeOnly ReadTimeValue(string propertyName, JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.String || !TimeOnly.TryParseExact(element.GetString(), "HH:mm:ss", out var time))
        {
            throw new ValidationException($"Schedule configuration field '{propertyName}' must contain HH:mm:ss values.");
        }

        return time;
    }

    private static IReadOnlySet<DayOfWeek> ReadDaysOfWeek(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var value) || value.ValueKind != JsonValueKind.Array)
        {
            throw new ValidationException($"Schedule configuration field '{propertyName}' must be an array.");
        }

        var result = new HashSet<DayOfWeek>();
        foreach (var item in value.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.String || !Enum.TryParse<DayOfWeek>(item.GetString(), true, out var dayOfWeek))
            {
                throw new ValidationException($"Schedule configuration field '{propertyName}' contains an invalid day value.");
            }

            result.Add(dayOfWeek);
        }

        return result;
    }

    private static string ReadRequiredString(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var value) || value.ValueKind != JsonValueKind.String)
        {
            throw new ValidationException($"Schedule configuration field '{propertyName}' is required.");
        }

        var result = value.GetString();
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new ValidationException($"Schedule configuration field '{propertyName}' is required.");
        }

        return result;
    }

    private static int ReadRequiredInt(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var value) || value.ValueKind != JsonValueKind.Number || !value.TryGetInt32(out var result))
        {
            throw new ValidationException($"Schedule configuration field '{propertyName}' must be an integer.");
        }

        return result;
    }

    private static DateTime? GetNextDailyOccurrence(TimeZoneInfo timeZone, DateTime utcAfterExclusive, TimeOnly localTime)
    {
        var localStart = TimeZoneInfo.ConvertTimeFromUtc(utcAfterExclusive, timeZone);
        for (var offset = 0; offset < 370; offset++)
        {
            var date = DateOnly.FromDateTime(localStart.Date).AddDays(offset);
            var candidate = CreateUtcCandidate(timeZone, date, localTime);
            if (candidate.HasValue && candidate.Value > utcAfterExclusive)
            {
                return candidate.Value;
            }
        }

        return null;
    }

    private static DateTime? GetNextWeeklyOccurrence(TimeZoneInfo timeZone, DateTime utcAfterExclusive, TimeOnly localTime, IReadOnlySet<DayOfWeek> daysOfWeek)
    {
        var localStart = TimeZoneInfo.ConvertTimeFromUtc(utcAfterExclusive, timeZone);
        for (var offset = 0; offset < 370; offset++)
        {
            var date = DateOnly.FromDateTime(localStart.Date).AddDays(offset);
            if (!daysOfWeek.Contains(date.DayOfWeek))
            {
                continue;
            }

            var candidate = CreateUtcCandidate(timeZone, date, localTime);
            if (candidate.HasValue && candidate.Value > utcAfterExclusive)
            {
                return candidate.Value;
            }
        }

        return null;
    }

    private static DateTime? GetNextRepeatInWindowOccurrence(TimeZoneInfo timeZone, DateTime utcAfterExclusive, TimeOnly windowStart, TimeOnly windowEnd, int intervalMinutes)
    {
        var localStart = TimeZoneInfo.ConvertTimeFromUtc(utcAfterExclusive, timeZone);
        for (var dayOffset = 0; dayOffset < 370; dayOffset++)
        {
            var date = DateOnly.FromDateTime(localStart.Date).AddDays(dayOffset);
            for (var candidateTime = windowStart; candidateTime < windowEnd; candidateTime = candidateTime.AddMinutes(intervalMinutes))
            {
                var candidate = CreateUtcCandidate(timeZone, date, candidateTime);
                if (candidate.HasValue && candidate.Value > utcAfterExclusive)
                {
                    return candidate.Value;
                }
            }
        }

        return null;
    }

    private static DateTime? GetNextMultipleTimesOccurrence(TimeZoneInfo timeZone, DateTime utcAfterExclusive, IReadOnlyList<TimeOnly> times)
    {
        var localStart = TimeZoneInfo.ConvertTimeFromUtc(utcAfterExclusive, timeZone);
        for (var dayOffset = 0; dayOffset < 370; dayOffset++)
        {
            var date = DateOnly.FromDateTime(localStart.Date).AddDays(dayOffset);
            foreach (var time in times)
            {
                var candidate = CreateUtcCandidate(timeZone, date, time);
                if (candidate.HasValue && candidate.Value > utcAfterExclusive)
                {
                    return candidate.Value;
                }
            }
        }

        return null;
    }

    private async Task<DateTime?> GetNextLastBankDayOccurrenceAsync(
        TimeZoneInfo timeZone,
        DateTime utcAfterExclusive,
        TimeOnly localTime,
        string calendarCode,
        CancellationToken cancellationToken)
    {
        var localStart = TimeZoneInfo.ConvertTimeFromUtc(utcAfterExclusive, timeZone);
        var monthCursor = new DateOnly(localStart.Year, localStart.Month, 1);

        for (var monthOffset = 0; monthOffset < 24; monthOffset++)
        {
            var targetMonth = monthCursor.AddMonths(monthOffset);
            var firstDay = new DateOnly(targetMonth.Year, targetMonth.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            var holidays = await administrationDA.GetHolidayCalendarEntriesAsync(calendarCode, firstDay, lastDay, cancellationToken);
            var holidaySet = holidays.Select(x => x.Date).ToHashSet();
            for (var date = lastDay; date >= firstDay; date = date.AddDays(-1))
            {
                if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday || holidaySet.Contains(date))
                {
                    continue;
                }

                var candidate = CreateUtcCandidate(timeZone, date, localTime);
                if (candidate.HasValue && candidate.Value > utcAfterExclusive)
                {
                    return candidate.Value;
                }

                break;
            }
        }

        return null;
    }

    private static DateTime? CreateUtcCandidate(TimeZoneInfo timeZone, DateOnly date, TimeOnly time)
    {
        var local = date.ToDateTime(time, DateTimeKind.Unspecified);
        if (timeZone.IsInvalidTime(local))
        {
            return null;
        }

        if (timeZone.IsAmbiguousTime(local))
        {
            return timeZone.GetAmbiguousTimeOffsets(local).Select(offset => new DateTimeOffset(local, offset).UtcDateTime).Min();
        }

        return TimeZoneInfo.ConvertTimeToUtc(local, timeZone);
    }
}
