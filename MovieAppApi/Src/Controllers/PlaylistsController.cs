using MovieAppApi.Src.Core.Database.Entities;
using MovieAppApi.Src.Core.Repositories;
using MovieAppApi.Src.Views.DTO.Playlists;
using Microsoft.AspNetCore.Mvc;

namespace MovieAppApi.Src.Controllers;

/// <summary>
/// Controller for managing user playlists and their associated movies.
/// Provides endpoints for creating, retrieving, updating, and deleting playlists,
/// as well as managing the movies within each playlist.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PlaylistsController : BaseController<PlaylistsController>
{
    private readonly IPlaylistRepository _playlistRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaylistsController"/> class.
    /// </summary>
    /// <param name="playlistRepository">The repository for accessing playlist data.</param>
    /// <param name="logger">The logger instance for recording controller operations.</param>
    public PlaylistsController(IPlaylistRepository playlistRepository, ILogger<PlaylistsController> logger) : base(logger)
    {
        _playlistRepository = playlistRepository;
    }

    /// <summary>
    /// Retrieves all available playlists.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> containing a list of all playlists.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<PlaylistResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Getting all playlists");
        var playlists = await _playlistRepository.GetAllAsync();
        var dtos = playlists.Select(MapToDto).ToList();
        return Ok(dtos);
    }

    /// <summary>
    /// Retrieves a specific playlist by its ID.
    /// </summary>
    /// <param name="id">The ID of the playlist to retrieve.</param>
    /// <returns>An <see cref="OkObjectResult"/> containing the playlist details if found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlaylistResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id);
        if (playlist == null) return NotFound(new { message = "Playlist not found" });
        return Ok(MapToDto(playlist));
    }

    /// <summary>
    /// Creates a new playlist with the specified name.
    /// </summary>
    /// <param name="request">The request containing the playlist name.</param>
    /// <returns>A <see cref="CreatedAtActionResult"/> with the newly created playlist.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PlaylistResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePlaylistRequestDto request)
    {
        var playlist = new PlaylistEntity { Name = request.Name };
        var created = await _playlistRepository.CreateAsync(playlist);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }

    /// <summary>
    /// Adds a movie to an existing playlist.
    /// </summary>
    /// <param name="id">The ID of the playlist.</param>
    /// <param name="request">The request containing the movie details (TMDB ID and title).</param>
    /// <returns>An <see cref="OkResult"/> if the movie was successfully added.</returns>
    [HttpPost("{id}/movies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddMovie(int id, [FromBody] AddMovieToPlaylistRequestDto request)
    {
        try
        {
            var movieEntity = new PlaylistMovieEntity
            {
                TmdbId = request.TmdbId,
                Title = request.Title,
                PlaylistId = id
            };
            await _playlistRepository.AddMovieAsync(id, movieEntity);
            Logger.LogInformation("Added movie '{MovieTitle}' to playlist {PlaylistId}", request.Title, id);
            return Ok(new { message = "Movie added successfully" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Playlist not found" });
        }
    }

    /// <summary>
    /// Updates the name of an existing playlist.
    /// </summary>
    /// <param name="id">The ID of the playlist to update.</param>
    /// <param name="request">The request containing the new playlist name.</param>
    /// <returns>An <see cref="OkObjectResult"/> with the updated playlist.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PlaylistResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePlaylistRequestDto request)
    {
        Logger.LogInformation("Updating playlist {PlaylistId} with new name: {NewName}", id, request.Name);
        try
        {
            var updated = await _playlistRepository.UpdateAsync(id, request.Name);
            return Ok(MapToDto(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Playlist not found" });
        }
    }

    /// <summary>
    /// Deletes a playlist by its ID.
    /// </summary>
    /// <param name="id">The ID of the playlist to delete.</param>
    /// <returns>A <see cref="NoContentResult"/> if successfully deleted.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _playlistRepository.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = "Playlist not found" });
        return NoContent();
    }

    /// <summary>
    /// Removes a movie from a playlist.
    /// </summary>
    /// <param name="id">The ID of the playlist.</param>
    /// <param name="tmdbId">The TMDB ID of the movie to remove.</param>
    /// <returns>A <see cref="NoContentResult"/> if successfully removed.</returns>
    [HttpDelete("{id}/movies/{tmdbId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveMovie(int id, int tmdbId)
    {
        Logger.LogInformation("Removing movie {TmdbId} from playlist {PlaylistId}", tmdbId, id);
        var result = await _playlistRepository.RemoveMovieAsync(id, tmdbId);
        if (!result)
        {
            Logger.LogWarning("Movie {TmdbId} not found in playlist {PlaylistId}", tmdbId, id);
            return NotFound(new { message = "Movie not found in playlist" });
        }
        return NoContent();
    }

    /// <summary>
    /// Helper method to map a PlaylistEntity to a PlaylistResponseDto.
    /// </summary>
    /// <param name="entity">The playlist entity to map.</param>
    /// <returns>A mapped <see cref="PlaylistResponseDto"/> instance.</returns>
    private static PlaylistResponseDto MapToDto(PlaylistEntity entity)
    {
        return new PlaylistResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            CreatedAt = entity.CreatedAt,
            Movies = entity.Movies.Select(m => new PlaylistMovieResponseDto
            {
                Id = m.Id,
                TmdbId = m.TmdbId,
                Title = m.Title,
                AddedAt = m.AddedAt
            }).ToList()
        };
    }
}
