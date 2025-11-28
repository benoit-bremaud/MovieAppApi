namespace MovieAppApi.Src.Views.DTO.Playlists;

/// <summary>
/// Data Transfer Object for a movie within a playlist response.
/// Contains movie details and metadata about when it was added to the playlist.
/// </summary>
public class PlaylistMovieResponseDto
{
    /// <summary>
    /// Gets or sets the database ID of the movie in the playlist.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the TMDB (The Movie Database) ID of the movie.
    /// Used for reference to the external TMDB API and data.
    /// </summary>
    public int TmdbId { get; set; }

    /// <summary>
    /// Gets or sets the title of the movie.
    /// This is a required field for display purposes.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the movie was added to the playlist (UTC).
    /// </summary>
    public DateTime AddedAt { get; set; }
}
