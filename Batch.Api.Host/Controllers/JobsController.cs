using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Services;
using Microsoft.AspNetCore.Mvc;

namespace Batch.Api.Host.Controllers;

/// <summary>
/// Provides CRUD endpoints for batch job definitions.
/// </summary>
[ApiController]
[Route("jobs")]
public sealed class JobsController(IAdministrationService administrationService) : ControllerBase
{
    /// <summary>
    /// Gets a paged list of batch jobs.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<BatchJob>), StatusCodes.Status200OK)]
    public Task<PagedResult<BatchJob>> GetJobsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default) =>
        administrationService.GetBatchJobsAsync(page, pageSize, cancellationToken);

    /// <summary>
    /// Creates a batch job.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BatchJob), StatusCodes.Status201Created)]
    public async Task<ActionResult<BatchJob>> CreateJobAsync([FromBody] BatchJobUpsertModel model, CancellationToken cancellationToken)
    {
        var result = await administrationService.CreateBatchJobAsync(model, cancellationToken);
        return Created($"/jobs/{result.Id}", result);
    }

    /// <summary>
    /// Replaces the mutable fields of a batch job.
    /// </summary>
    [HttpPut("{batchJobId:int}")]
    [ProducesResponseType(typeof(BatchJob), StatusCodes.Status200OK)]
    public Task<BatchJob> UpdateJobAsync(int batchJobId, [FromBody] BatchJobUpsertModel model, CancellationToken cancellationToken) =>
        administrationService.UpdateBatchJobAsync(batchJobId, model, cancellationToken);
}
