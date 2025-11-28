using Microsoft.AspNetCore.Mvc;

namespace MovieAppApi.Src.Controllers;

/// <summary>
/// Controller for monitoring and checking the health status of the MovieApp API.
/// Provides a simple endpoint that verifies the API service is running and responding correctly.
/// </summary>
public class HealthController : BaseController<HealthController>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HealthController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for recording health check operations.</param>
    public HealthController(ILogger<HealthController> logger) : base(logger)
    {
    }

    /// <summary>
    /// Returns the current health status of the API.
    /// </summary>
    /// <returns>
    /// An <see cref="OkObjectResult"/> containing the health status with timestamp and version information.
    /// Response includes: status ("Healthy"), timestamp (UTC), and API version.
    /// </returns>
    /// <remarks>
    /// This endpoint is typically used by load balancers, orchestration systems (Kubernetes),
    /// and monitoring dashboards to verify API availability and responsiveness.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        Logger.LogInformation("🏥 Health check requested");
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
        });
    }
}
