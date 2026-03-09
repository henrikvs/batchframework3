using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Services;
using Microsoft.AspNetCore.Mvc;

namespace Batch.Api.Host.Controllers;

/// <summary>
/// Provides CRUD endpoints for runner profiles.
/// </summary>
[ApiController]
[Route("runner-profiles")]
public sealed class RunnerProfilesController(IAdministrationService administrationService) : ControllerBase
{
    /// <summary>
    /// Gets a paged list of runner profiles.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<RunnerProfile>), StatusCodes.Status200OK)]
    public Task<PagedResult<RunnerProfile>> GetRunnerProfilesAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default) =>
        administrationService.GetRunnerProfilesAsync(page, pageSize, cancellationToken);

    /// <summary>
    /// Creates a runner profile.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RunnerProfile), StatusCodes.Status201Created)]
    public async Task<ActionResult<RunnerProfile>> CreateRunnerProfileAsync([FromBody] RunnerProfileUpsertModel model, CancellationToken cancellationToken)
    {
        var result = await administrationService.CreateRunnerProfileAsync(model, cancellationToken);
        return Created($"/runner-profiles/{result.Id}", result);
    }

    /// <summary>
    /// Replaces the mutable fields of a runner profile.
    /// </summary>
    [HttpPut("{runnerProfileId:int}")]
    [ProducesResponseType(typeof(RunnerProfile), StatusCodes.Status200OK)]
    public Task<RunnerProfile> UpdateRunnerProfileAsync(int runnerProfileId, [FromBody] RunnerProfileUpsertModel model, CancellationToken cancellationToken) =>
        administrationService.UpdateRunnerProfileAsync(runnerProfileId, model, cancellationToken);
}
