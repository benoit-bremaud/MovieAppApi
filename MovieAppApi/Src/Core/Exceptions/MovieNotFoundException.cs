namespace MovieAppApi.Src.Core.Exceptions;

/// <summary>
/// Exception thrown when a requested movie cannot be found in the TMDB API or database.
/// Used by movie services and controllers to indicate that a movie lookup operation failed.
/// </summary>
public class MovieNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MovieNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message describing why the movie was not found.</param>
    public MovieNotFoundException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieNotFoundException"/> class with an inner exception.
    /// </summary>
    /// <param name="message">The error message describing why the movie was not found.</param>
    /// <param name="innerException">The inner exception that caused this exception.</param>
    public MovieNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
