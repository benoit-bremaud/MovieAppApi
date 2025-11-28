namespace MovieAppApi.Src.Views.DTO.Playlists;

/// <summary>
/// Data Transfer Object for a playlist response.
/// Contains complete playlist information including metadata and associated movies.
/// </summary>
public class PlaylistResponseDto
{
    /// <summary>
    /// Gets or sets the unique database ID of the playlist.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the playlist.
    /// This is a required field for identification.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the playlist was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the count of movies in the playlist.
    /// Computed from the Movies collection size.
    /// </summary>
    public int MovieCount => Movies.Count;

    /// <summary>
    /// Gets or sets the collection of movies in the playlist.
    /// Each item contains movie details and addition timestamp.
    /// </summary>
    public List<PlaylistMovieResponseDto> Movies { get; set; } = new();
}
