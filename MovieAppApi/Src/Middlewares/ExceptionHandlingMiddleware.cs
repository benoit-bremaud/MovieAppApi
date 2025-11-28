using System.Net;
using System.Text.Json;
using MovieAppApi.Src.Core.Exceptions;

namespace MovieAppApi.Src.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

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
