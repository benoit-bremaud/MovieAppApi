using MovieAppApi.Src.Core.Database.Entities;

namespace MovieAppApi.Src.Core.Repositories;

/// <summary>
/// Repository interface for managing Playlist entities
/// </summary>
public interface IPlaylistRepository
{
    /// <summary>
    /// Gets all playlists with their movies
    /// </summary>
    Task<List<PlaylistEntity>> GetAllAsync();

    /// <summary>
    /// Gets a specific playlist by ID including its movies
    /// </summary>
    Task<PlaylistEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new playlist
    /// </summary>
    Task<PlaylistEntity> CreateAsync(PlaylistEntity playlist);

    /// <summary>
    /// Adds a movie to an existing playlist
    /// </summary>
    Task AddMovieAsync(int playlistId, PlaylistMovieEntity movie);

    /// <summary>
    /// Deletes a playlist by ID
    /// </summary>
    Task<bool> DeleteAsync(int id);
}
