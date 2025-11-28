using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Src.Views.DTO.Playlists;

public class CreatePlaylistRequestDto
{
    [Required(ErrorMessage = "Playlist name is required")]
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    public required string Name { get; set; }
}
