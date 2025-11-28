using Microsoft.AspNetCore.Mvc;

namespace MovieAppApi.Src.Controllers;

/// <summary>
/// Abstract base controller providing common logging functionality for all API controllers.
/// Implements standard REST API patterns with centralized logging and error handling.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller
{
    /// <summary>
    /// Logger instance used for recording operations, warnings, and errors across the controller.
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance to be used for logging controller operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when the logger parameter is null.</exception>
    protected BaseController(ILogger logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
}
