using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Src.Views.DTO.Playlists;

/// <summary>
/// Data Transfer Object for adding a movie to a playlist.
/// Contains the required TMDB ID and movie title for the addition operation.
/// </summary>
public class AddMovieToPlaylistRequestDto
{
    /// <summary>
    /// Gets or sets the TMDB ID of the movie to add to the playlist.
    /// This is a required field used to identify the movie in TMDB.
    /// </summary>
    [Required]
    public int TmdbId { get; set; }

    /// <summary>
    /// Gets or sets the title of the movie to add to the playlist.
    /// This is a required field for display purposes.
    /// </summary>
    [Required]
    public required string Title { get; set; }
}
