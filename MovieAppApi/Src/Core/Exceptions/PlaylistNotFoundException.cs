namespace MovieAppApi.Src.Core.Exceptions;

/// <summary>
/// Exception thrown when a requested playlist cannot be found in the database.
/// Used by playlist repository and controllers to indicate that a playlist lookup operation failed.
/// </summary>
public class PlaylistNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlaylistNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The error message describing why the playlist was not found.</param>
    public PlaylistNotFoundException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaylistNotFoundException"/> class with an inner exception.
    /// </summary>
    /// <param name="message">The error message describing why the playlist was not found.</param>
    /// <param name="innerException">The inner exception that caused this exception.</param>
    public PlaylistNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
