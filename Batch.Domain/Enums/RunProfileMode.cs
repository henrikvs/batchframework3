namespace Batch.Domain.Enums;

/// <summary>
/// Represents the reusable process launch behavior configured on a runner profile.
/// </summary>
public enum RunProfileMode
{
    /// <summary>
    /// Uses the legacy VB launch contract.
    /// </summary>
    LegacyVbStyle,

    /// <summary>
    /// Uses the .NET batch launch contract.
    /// </summary>
    DotNetBatchStyle,

    /// <summary>
    /// Uses a batch or command file launch contract.
    /// </summary>
    BatchFileStyle,

    /// <summary>
    /// Uses a PowerShell launch contract.
    /// </summary>
    PowerShellStyle,

    /// <summary>
    /// Uses a custom argument template.
    /// </summary>
    CustomTemplate,
}
