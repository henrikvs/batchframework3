using Batch.Domain.Contracts;
using Batch.Domain.Entities;
using Batch.Services;
using Microsoft.AspNetCore.Mvc;

namespace Batch.Api.Host.Controllers;

/// <summary>
/// Provides batch schedule endpoints.
/// </summary>
[ApiController]
public sealed class BatchSchedulesController(IAdministrationService administrationService) : ControllerBase
{
    /// <summary>
    /// Gets a paged list of schedules for a batch job.
    /// </summary>
    [HttpGet("jobs/{batchJobId:int}/batch-schedules")]
    [ProducesResponseType(typeof(PagedResult<BatchSchedule>), StatusCodes.Status200OK)]
    public Task<PagedResult<BatchSchedule>> GetBatchSchedulesAsync(int batchJobId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default) =>
        administrationService.GetBatchSchedulesAsync(batchJobId, page, pageSize, cancellationToken);

    /// <summary>
    /// Creates a schedule for a batch job.
    /// </summary>
    [HttpPost("jobs/{batchJobId:int}/batch-schedules")]
    [ProducesResponseType(typeof(BatchSchedule), StatusCodes.Status201Created)]
    public async Task<ActionResult<BatchSchedule>> CreateBatchScheduleAsync(int batchJobId, [FromBody] BatchScheduleUpsertModel model, CancellationToken cancellationToken)
    {
        var result = await administrationService.CreateBatchScheduleAsync(batchJobId, model, cancellationToken);
        return Created($"/batch-schedules/{result.Id}", result);
    }

    /// <summary>
    /// Replaces the mutable fields of a schedule.
    /// </summary>
    [HttpPut("batch-schedules/{batchScheduleId:int}")]
    [ProducesResponseType(typeof(BatchSchedule), StatusCodes.Status200OK)]
    public Task<BatchSchedule> UpdateBatchScheduleAsync(int batchScheduleId, [FromBody] BatchScheduleUpsertModel model, CancellationToken cancellationToken) =>
        administrationService.UpdateBatchScheduleAsync(batchScheduleId, model, cancellationToken);
}
