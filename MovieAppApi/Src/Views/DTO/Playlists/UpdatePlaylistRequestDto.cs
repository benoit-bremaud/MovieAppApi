using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Src.Views.DTO.Playlists;

/// <summary>
/// Data Transfer Object for updating an existing playlist.
/// Contains the new name for the playlist with validation constraints.
/// </summary>
public class UpdatePlaylistRequestDto
{
    /// <summary>
    /// Gets or sets the new name for the playlist.
    /// This is a required field that cannot be empty.
    /// </summary>
    [Required(ErrorMessage = "Playlist name is required")]
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    public required string Name { get; set; }
}
