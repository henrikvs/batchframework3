using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Services;
using Microsoft.AspNetCore.Mvc;

namespace Batch.Api.Host.Controllers;

/// <summary>
/// Provides CRUD endpoints for tenants.
/// </summary>
[ApiController]
[Route("tenants")]
public sealed class TenantsController(IAdministrationService administrationService) : ControllerBase
{
    /// <summary>
    /// Gets a paged list of tenants.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<Tenant>), StatusCodes.Status200OK)]
    public Task<PagedResult<Tenant>> GetTenantsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default) =>
        administrationService.GetTenantsAsync(page, pageSize, cancellationToken);

    /// <summary>
    /// Creates a tenant.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Tenant), StatusCodes.Status201Created)]
    public async Task<ActionResult<Tenant>> CreateTenantAsync([FromBody] TenantUpsertModel model, CancellationToken cancellationToken)
    {
        var result = await administrationService.CreateTenantAsync(model, cancellationToken);
        return Created($"/tenants/{result.Id}", result);
    }

    /// <summary>
    /// Replaces the mutable fields of a tenant.
    /// </summary>
    [HttpPut("{tenantId:int}")]
    [ProducesResponseType(typeof(Tenant), StatusCodes.Status200OK)]
    public Task<Tenant> UpdateTenantAsync(int tenantId, [FromBody] TenantUpsertModel model, CancellationToken cancellationToken) =>
        administrationService.UpdateTenantAsync(tenantId, model, cancellationToken);
}
