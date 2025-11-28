using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Src.Views.DTO.Playlists;

/// <summary>
/// Data Transfer Object for creating a new playlist.
/// Contains the playlist name with validation constraints.
/// </summary>
public class CreatePlaylistRequestDto
{
    /// <summary>
    /// Gets or sets the name for the new playlist.
    /// This is a required field that cannot be empty.
    /// </summary>
    [Required(ErrorMessage = "Playlist name is required")]
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    public required string Name { get; set; }
}
