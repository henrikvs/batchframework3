namespace Batch.Domain.Enums;

/// <summary>
/// Represents whether a tenant may participate in scheduling and execution.
/// </summary>
public enum TenStatus
{
    /// <summary>
    /// The tenant cannot participate in scheduling or execution.
    /// </summary>
    Disabled,

    /// <summary>
    /// The tenant can participate in scheduling and execution.
    /// </summary>
    Active,
}
