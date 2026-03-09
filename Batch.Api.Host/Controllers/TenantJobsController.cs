using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Services;
using Microsoft.AspNetCore.Mvc;

namespace Batch.Api.Host.Controllers;

/// <summary>
/// Provides tenant-job configuration endpoints.
/// </summary>
[ApiController]
[Route("tenants/{tenantId:int}/jobs")]
public sealed class TenantJobsController(IAdministrationService administrationService) : ControllerBase
{
    /// <summary>
    /// Gets a paged list of tenant jobs for a tenant.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TenantJob>), StatusCodes.Status200OK)]
    public Task<PagedResult<TenantJob>> GetTenantJobsAsync(int tenantId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default) =>
        administrationService.GetTenantJobsAsync(tenantId, page, pageSize, cancellationToken);

    /// <summary>
    /// Creates or replaces a tenant-job configuration.
    /// </summary>
    [HttpPut("{batchJobId:int}")]
    [ProducesResponseType(typeof(TenantJob), StatusCodes.Status200OK)]
    public Task<TenantJob> UpsertTenantJobAsync(int tenantId, int batchJobId, [FromBody] TenantJobUpsertModel model, CancellationToken cancellationToken) =>
        administrationService.UpsertTenantJobAsync(tenantId, batchJobId, model, cancellationToken);
}
