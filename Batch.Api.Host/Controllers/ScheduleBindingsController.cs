using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Services;
using Microsoft.AspNetCore.Mvc;

namespace Batch.Api.Host.Controllers;

/// <summary>
/// Provides tenant-job schedule binding endpoints.
/// </summary>
[ApiController]
[Route("tenants/{tenantId:int}/jobs/{batchJobId:int}/schedule-bindings")]
public sealed class ScheduleBindingsController(IAdministrationService administrationService) : ControllerBase
{
    /// <summary>
    /// Gets a paged list of schedule bindings for a tenant job.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TenantJobScheduleBinding>), StatusCodes.Status200OK)]
    public Task<PagedResult<TenantJobScheduleBinding>> GetScheduleBindingsAsync(int tenantId, int batchJobId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default) =>
        administrationService.GetScheduleBindingsAsync(tenantId, batchJobId, page, pageSize, cancellationToken);

    /// <summary>
    /// Creates a schedule binding for a tenant job.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TenantJobScheduleBinding), StatusCodes.Status201Created)]
    public async Task<ActionResult<TenantJobScheduleBinding>> CreateScheduleBindingAsync(int tenantId, int batchJobId, [FromBody] TenantJobScheduleBindingUpsertModel model, CancellationToken cancellationToken)
    {
        var result = await administrationService.CreateScheduleBindingAsync(tenantId, batchJobId, model, cancellationToken);
        return Created($"/tenants/{tenantId}/jobs/{batchJobId}/schedule-bindings/{result.Id}", result);
    }
}
