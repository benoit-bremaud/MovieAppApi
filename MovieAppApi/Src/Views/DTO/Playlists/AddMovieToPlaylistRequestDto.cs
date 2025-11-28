using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Src.Views.DTO.Playlists;

public class AddMovieToPlaylistRequestDto
{
    [Required]
    public int TmdbId { get; set; }

    [Required]
    public required string Title { get; set; }
}
