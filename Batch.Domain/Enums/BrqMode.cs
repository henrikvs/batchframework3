namespace Batch.Domain.Enums;

/// <summary>
/// Represents how a run request expands into job instances.
/// </summary>
public enum BrqMode
{
    /// <summary>
    /// Creates a single target job instance.
    /// </summary>
    SingleJob,

    /// <summary>
    /// Creates the target job and prerequisite jobs.
    /// </summary>
    JobWithPrereqs,

    /// <summary>
    /// Creates a chain-oriented run request.
    /// </summary>
    Chain,

    /// <summary>
    /// Targets multiple tenants at once.
    /// </summary>
    OpsMultiTenant,
}
