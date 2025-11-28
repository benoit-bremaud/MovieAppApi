namespace MovieAppApi.Src.Views.DTO.Playlists;

public class PlaylistResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int MovieCount => Movies.Count;
    public List<PlaylistMovieResponseDto> Movies { get; set; } = new();
}
