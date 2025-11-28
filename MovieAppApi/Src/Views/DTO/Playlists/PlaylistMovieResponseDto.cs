namespace MovieAppApi.Src.Views.DTO.Playlists;

public class PlaylistMovieResponseDto
{
    public int Id { get; set; }
    public int TmdbId { get; set; }
    public required string Title { get; set; }
    public DateTime AddedAt { get; set; }
}
