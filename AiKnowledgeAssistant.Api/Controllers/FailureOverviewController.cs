using AiKnowledgeAssistant.Application.Failures.Implementations;
using AiKnowledgeAssistant.Application.Failures.Models;
using Microsoft.AspNetCore.Mvc;

namespace AiKnowledgeAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/failures/overview")]
    public sealed class FailureOverviewController : ControllerBase
    {
        private readonly FailureOverviewService _failureOverviewService;
        private readonly ILogger<FailureOverviewController> _logger;

        public FailureOverviewController(
            FailureOverviewService failureOverviewService,
            ILogger<FailureOverviewController> logger)
        {
            _failureOverviewService = failureOverviewService;
            _logger = logger;
        }

        [HttpGet("{jobId}")]
        [ProducesResponseType(typeof(FailureOverviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOverview(
            string jobId,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return BadRequest("JobId is required.");
            }

            try
            {
                FailureOverviewResponse response =
                    await _failureOverviewService.GetOverviewAsync(jobId);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "JobId {JobId} not found", jobId);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failure overview conflict for JobId {JobId}", jobId);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while generating failure overview for JobId {JobId}", jobId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

}
