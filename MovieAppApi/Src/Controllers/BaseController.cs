using Microsoft.AspNetCore.Mvc;

namespace MovieAppApi.Src.Controllers;

/// <summary>
/// Generic abstract base controller providing common logging functionality for all API controllers.
/// Implements standard REST API patterns with centralized, type-safe logging and error handling.
/// </summary>
/// <typeparam name="T">The type of the derived controller class, used for generic logging categorization.</typeparam>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<T> : Controller
{
    /// <summary>
    /// Logger instance used for recording operations, warnings, and errors specific to the derived controller type.
    /// Generic logger provides type-safe, categorized logging entries in application logs.
    /// </summary>
    protected readonly ILogger<T> Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseController{T}"/> class.
    /// </summary>
    /// <param name="logger">The type-safe logger instance to be used for logging controller operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when the logger parameter is null.</exception>
    protected BaseController(ILogger<T> logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
}
