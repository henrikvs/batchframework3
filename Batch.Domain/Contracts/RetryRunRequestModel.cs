namespace Batch.Domain.Contracts;

/// <summary>
/// Represents the optional override payload used when retrying a failed or cancelled instance.
/// </summary>
public sealed class RetryRunRequestModel
{
    /// <summary>
    /// Gets or sets the optional execution context override.
    /// </summary>
    public string? ExecutionContextJson { get; set; }

    /// <summary>
    /// Gets or sets the user or system name that created the retry request.
    /// </summary>
    public string? CreatedBy { get; set; }
}
