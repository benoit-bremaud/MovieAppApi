using System.Net;
using System.Text.Json;
using MovieAppApi.Src.Core.Exceptions;

namespace MovieAppApi.Src.Middlewares;

/// <summary>
/// Middleware for centralized exception handling in the HTTP pipeline.
/// Catches unhandled exceptions, logs them, and returns standardized error responses to clients.
/// Maps custom exceptions to appropriate HTTP status codes and error messages.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger instance for recording exceptions.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Processes the HTTP request and handles any exceptions that occur in the pipeline.
    /// </summary>
    /// <param name="context">The HTTP context containing request and response information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles the exception by setting the appropriate HTTP status code and returning a standardized error response.
    /// Maps custom exceptions to their corresponding HTTP status codes.
    /// </summary>
    /// <param name="context">The HTTP context for writing the error response.</param>
    /// <param name="exception">The exception that occurred during request processing.</param>
    /// <returns>A completed task.</returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new { message = "An error occurred", details = exception.Message };

        switch (exception)
        {
            case MovieNotFoundException ex:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response = new { message = "Movie not found", details = ex.Message };
                break;

            case PlaylistNotFoundException ex:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response = new { message = "Playlist not found", details = ex.Message };
                break;

            case TmdbApiException ex:
                context.Response.StatusCode = ex.StatusCode ?? StatusCodes.Status503ServiceUnavailable;
                response = new { message = "TMDB API error", details = ex.Message };
                break;

            case ArgumentException ex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = new { message = "Invalid argument", details = ex.Message };
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = new { message = "Internal server error", details = "An unexpected error occurred" };
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}
