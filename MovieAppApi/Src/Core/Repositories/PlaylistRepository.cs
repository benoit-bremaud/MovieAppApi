using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Core.Database;
using MovieAppApi.Src.Core.Database.Entities;

namespace MovieAppApi.Src.Core.Repositories;

/// <summary>
/// Implementation of IPlaylistRepository using EF Core
/// </summary>
public class PlaylistRepository : IPlaylistRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PlaylistRepository> _logger;

    public PlaylistRepository(AppDbContext context, ILogger<PlaylistRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<PlaylistEntity>> GetAllAsync()
    {
        return await _context.Playlists
            .Include(p => p.Movies)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<PlaylistEntity?> GetByIdAsync(int id)
    {
        return await _context.Playlists
            .Include(p => p.Movies)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PlaylistEntity> CreateAsync(PlaylistEntity playlist)
    {
        _logger.LogInformation("Creating new playlist: {Name}", playlist.Name);
        
        await _context.Playlists.AddAsync(playlist);
        await _context.SaveChangesAsync();
        
        return playlist;
    }

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

    public async Task<bool> DeleteAsync(int id)
    {
        var playlist = await _context.Playlists.FindAsync(id);
        if (playlist == null) return false;

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();
        return true;
    }

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

