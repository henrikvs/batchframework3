using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Services;
using Microsoft.AspNetCore.Mvc;

namespace Batch.Api.Host.Controllers;

/// <summary>
/// Provides instance query and retry endpoints.
/// </summary>
[ApiController]
public sealed class InstancesController(IJobInstanceService jobInstanceService, IRunRequestService runRequestService) : ControllerBase
{
    /// <summary>
    /// Gets a paged list of instances for a tenant.
    /// </summary>
    [HttpGet("tenants/{tenantId:int}/instances")]
    [ProducesResponseType(typeof(PagedResult<BatchJobInstance>), StatusCodes.Status200OK)]
    public Task<PagedResult<BatchJobInstance>> GetInstancesAsync(int tenantId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default) =>
        jobInstanceService.GetInstancesAsync(tenantId, page, pageSize, cancellationToken);

    /// <summary>
    /// Gets a specific instance by identifier.
    /// </summary>
    [HttpGet("instances/{instanceId:guid}")]
    [ProducesResponseType(typeof(BatchJobInstance), StatusCodes.Status200OK)]
    public Task<BatchJobInstance> GetInstanceAsync(Guid instanceId, CancellationToken cancellationToken) =>
        jobInstanceService.GetInstanceAsync(instanceId, cancellationToken);

    /// <summary>
    /// Creates a retry run request for an existing instance.
    /// </summary>
    [HttpPost("instances/{instanceId:guid}/retry")]
    [ProducesResponseType(typeof(BatchRunRequest), StatusCodes.Status200OK)]
    public Task<BatchRunRequest> RetryInstanceAsync(Guid instanceId, [FromBody] RetryRunRequestModel model, CancellationToken cancellationToken) =>
        runRequestService.RetryInstanceAsync(instanceId, model, cancellationToken);
}
