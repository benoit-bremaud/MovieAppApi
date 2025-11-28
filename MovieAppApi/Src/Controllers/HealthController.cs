using Microsoft.AspNetCore.Mvc;

namespace MovieAppApi.Src.Controllers;

/// <summary>
/// Controller for checking API health status
/// </summary>
public class HealthController : BaseController<HealthController>
{
    public HealthController(ILogger<HealthController> logger) : base(logger)
    {
    }

    /// <summary>
    /// Returns the status of the API
    /// </summary>
    /// <returns>OK status with timestamp</returns>
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
