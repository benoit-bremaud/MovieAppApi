namespace MovieAppApi.Src.Core.Exceptions;

/// <summary>
/// Exception thrown when the TMDB API returns an error response or fails to process a request.
/// Includes the HTTP status code for detailed error handling and debugging.
/// </summary>
public class TmdbApiException : Exception
{
    /// <summary>
    /// Gets or sets the HTTP status code returned by the TMDB API.
    /// May be null if the error occurred before receiving an HTTP response.
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TmdbApiException"/> class.
    /// </summary>
    /// <param name="message">The error message describing the TMDB API failure.</param>
    /// <param name="statusCode">The optional HTTP status code returned by the TMDB API.</param>
    public TmdbApiException(string message, int? statusCode = null) : base(message)
    {
        StatusCode = statusCode;
    }
}
