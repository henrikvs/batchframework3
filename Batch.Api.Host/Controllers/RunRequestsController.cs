using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Services;
using Microsoft.AspNetCore.Mvc;

namespace Batch.Api.Host.Controllers;

/// <summary>
/// Provides manual run request endpoints.
/// </summary>
[ApiController]
public sealed class RunRequestsController(IRunRequestService runRequestService) : ControllerBase
{
    /// <summary>
    /// Creates a manual run request for a tenant.
    /// </summary>
    [HttpPost("tenants/{tenantId:int}/requests")]
    [ProducesResponseType(typeof(BatchRunRequest), StatusCodes.Status201Created)]
    public async Task<ActionResult<BatchRunRequest>> CreateRunRequestAsync(int tenantId, [FromBody] ManualRunRequestModel model, CancellationToken cancellationToken)
    {
        var result = await runRequestService.CreateManualRunRequestAsync(tenantId, model, cancellationToken);
        return Created($"/requests/{result.Id}", result);
    }

    /// <summary>
    /// Gets a paged list of run requests for a tenant.
    /// </summary>
    [HttpGet("tenants/{tenantId:int}/requests")]
    [ProducesResponseType(typeof(PagedResult<BatchRunRequest>), StatusCodes.Status200OK)]
    public Task<PagedResult<BatchRunRequest>> GetRunRequestsAsync(int tenantId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default) =>
        runRequestService.GetRunRequestsAsync(tenantId, page, pageSize, cancellationToken);

    /// <summary>
    /// Cancels a planned run request.
    /// </summary>
    [HttpPost("requests/{runRequestId:guid}/cancel")]
    [ProducesResponseType(typeof(BatchRunRequest), StatusCodes.Status200OK)]
    public Task<BatchRunRequest> CancelRunRequestAsync(Guid runRequestId, CancellationToken cancellationToken) =>
        runRequestService.CancelRunRequestAsync(runRequestId, cancellationToken);
}
