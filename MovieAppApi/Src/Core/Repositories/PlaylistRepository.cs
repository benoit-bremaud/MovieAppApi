using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Core.Database;
using MovieAppApi.Src.Core.Database.Entities;

namespace MovieAppApi.Src.Core.Repositories;

/// <summary>
/// Implementation of <see cref="IPlaylistRepository"/> using Entity Framework Core.
/// Provides CRUD operations and data access methods for playlists and associated movies.
/// Manages database transactions and timestamp updates for audit tracking.
/// </summary>
public class PlaylistRepository : IPlaylistRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PlaylistRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaylistRepository"/> class.
    /// </summary>
    /// <param name="context">The Entity Framework Core database context.</param>
    /// <param name="logger">The logger instance for recording repository operations.</param>
    public PlaylistRepository(AppDbContext context, ILogger<PlaylistRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all playlists from the database with their associated movies.
    /// Results are ordered by creation date in descending order (newest first).
    /// </summary>
    /// <returns>A list of all <see cref="PlaylistEntity"/> objects with related movies included.</returns>
    public async Task<List<PlaylistEntity>> GetAllAsync()
    {
        return await _context.Playlists
            .Include(p => p.Movies)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific playlist by its ID with all associated movies.
    /// </summary>
    /// <param name="id">The ID of the playlist to retrieve.</param>
    /// <returns>The <see cref="PlaylistEntity"/> if found; otherwise null.</returns>
    public async Task<PlaylistEntity?> GetByIdAsync(int id)
    {
        return await _context.Playlists
            .Include(p => p.Movies)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Creates a new playlist in the database.
    /// </summary>
    /// <param name="playlist">The <see cref="PlaylistEntity"/> to create.</param>
    /// <returns>The created <see cref="PlaylistEntity"/> with database-generated ID.</returns>
    public async Task<PlaylistEntity> CreateAsync(PlaylistEntity playlist)
    {
        _logger.LogInformation("Creating new playlist: {Name}", playlist.Name);
        await _context.Playlists.AddAsync(playlist);
        await _context.SaveChangesAsync();
        return playlist;
    }

    /// <summary>
    /// Adds a movie to an existing playlist.
    /// Updates the playlist's modification timestamp.
    /// </summary>
    /// <param name="playlistId">The ID of the playlist to add the movie to.</param>
    /// <param name="movie">The <see cref="PlaylistMovieEntity"/> to add.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the playlist is not found.</exception>
    public async Task AddMovieAsync(int playlistId, PlaylistMovieEntity movie)
    {
        var playlist = await _context.Playlists.FindAsync(playlistId);
        if (playlist == null) throw new KeyNotFoundException($"Playlist {playlistId} not found");

        movie.PlaylistId = playlistId;
        await _context.PlaylistMovies.AddAsync(movie);

        // Update playlist timestamp
        playlist.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a playlist by its ID, including all associated movies (cascade delete).
    /// </summary>
    /// <param name="id">The ID of the playlist to delete.</param>
    /// <returns>True if the playlist was successfully deleted; false if not found.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var playlist = await _context.Playlists.FindAsync(id);
        if (playlist == null) return false;

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Removes a movie from a playlist by movie TMDB ID.
    /// Updates the playlist's modification timestamp.
    /// </summary>
    /// <param name="playlistId">The ID of the playlist.</param>
    /// <param name="tmdbId">The TMDB ID of the movie to remove.</param>
    /// <returns>True if the movie was successfully removed; false if not found in playlist.</returns>
    public async Task<bool> RemoveMovieAsync(int playlistId, int tmdbId)
    {
        var movie = await _context.PlaylistMovies
            .FirstOrDefaultAsync(m => m.PlaylistId == playlistId && m.TmdbId == tmdbId);
        if (movie == null) return false;

        _context.PlaylistMovies.Remove(movie);

        // Update playlist timestamp
        var playlist = await _context.Playlists.FindAsync(playlistId);
        if (playlist != null)
        {
            playlist.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Updates the name of an existing playlist.
    /// Updates the playlist's modification timestamp.
    /// </summary>
    /// <param name="id">The ID of the playlist to update.</param>
    /// <param name="newName">The new name for the playlist.</param>
    /// <returns>The updated <see cref="PlaylistEntity"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the playlist is not found.</exception>
    public async Task<PlaylistEntity> UpdateAsync(int id, string newName)
    {
        var playlist = await _context.Playlists.FindAsync(id);
        if (playlist == null) throw new KeyNotFoundException($"Playlist {id} not found");

        _logger.LogInformation("Updating playlist {PlaylistId} name to {NewName}", id, newName);
        playlist.Name = newName;
        playlist.UpdatedAt = DateTime.UtcNow;
        _context.Playlists.Update(playlist);
        await _context.SaveChangesAsync();
        return playlist;
    }
}
