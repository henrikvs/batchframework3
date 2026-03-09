namespace Batch.Domain.Enums;

/// <summary>
/// Represents how a business-date argument is appended to a process launch.
/// </summary>
public enum BajDateArgumentMode
{
    /// <summary>
    /// No date argument is appended.
    /// </summary>
    None,

    /// <summary>
    /// The formatted date is appended as a positional token.
    /// </summary>
    Positional,

    /// <summary>
    /// The name and formatted date are combined into one token.
    /// </summary>
    NamedValueSameToken,

    /// <summary>
    /// The name and formatted date are emitted as separate tokens.
    /// </summary>
    NamedSeparateToken,
}
