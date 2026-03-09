using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Services;
using Microsoft.AspNetCore.Mvc;

namespace Batch.Api.Host.Controllers;

/// <summary>
/// Provides tenant execution parameter endpoints.
/// </summary>
[ApiController]
[Route("tenants/{tenantId:int}/execution-parameters")]
public sealed class TenantExecutionParametersController(IAdministrationService administrationService) : ControllerBase
{
    /// <summary>
    /// Gets a paged list of tenant execution parameters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TenantExecutionParameter>), StatusCodes.Status200OK)]
    public Task<PagedResult<TenantExecutionParameter>> GetExecutionParametersAsync(int tenantId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default) =>
        administrationService.GetTenantExecutionParametersAsync(tenantId, page, pageSize, cancellationToken);

    /// <summary>
    /// Creates a tenant execution parameter.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TenantExecutionParameter), StatusCodes.Status201Created)]
    public async Task<ActionResult<TenantExecutionParameter>> CreateExecutionParameterAsync(int tenantId, [FromBody] TenantExecutionParameterUpsertModel model, CancellationToken cancellationToken)
    {
        var result = await administrationService.CreateTenantExecutionParameterAsync(tenantId, model, cancellationToken);
        return Created($"/tenants/{tenantId}/execution-parameters/{result.Id}", result);
    }

    /// <summary>
    /// Replaces the mutable fields of a tenant execution parameter.
    /// </summary>
    [HttpPut("{executionParameterId:int}")]
    [ProducesResponseType(typeof(TenantExecutionParameter), StatusCodes.Status200OK)]
    public Task<TenantExecutionParameter> UpdateExecutionParameterAsync(int tenantId, int executionParameterId, [FromBody] TenantExecutionParameterUpsertModel model, CancellationToken cancellationToken) =>
        administrationService.UpdateTenantExecutionParameterAsync(tenantId, executionParameterId, model, cancellationToken);
}
