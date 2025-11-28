using MovieAppApi.Src.Core.Database.Entities;
using MovieAppApi.Src.Core.Repositories;
using MovieAppApi.Src.Views.DTO.Playlists;
using Microsoft.AspNetCore.Mvc;

namespace MovieAppApi.Src.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlaylistsController : BaseController<PlaylistsController>
{
    private readonly IPlaylistRepository _playlistRepository;

    public PlaylistsController(IPlaylistRepository playlistRepository, ILogger<PlaylistsController> logger) : base(logger)
    {
        _playlistRepository = playlistRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<PlaylistResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Getting all playlists");
        var playlists = await _playlistRepository.GetAllAsync();
        
        var dtos = playlists.Select(MapToDto).ToList();
        
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlaylistResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id);
        if (playlist == null) return NotFound(new { message = "Playlist not found" });

        return Ok(MapToDto(playlist));
    }

    [HttpPost]
    [ProducesResponseType(typeof(PlaylistResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePlaylistRequestDto request)
    {
        var playlist = new PlaylistEntity { Name = request.Name };
        var created = await _playlistRepository.CreateAsync(playlist);
        
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }

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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _playlistRepository.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = "Playlist not found" });

        return NoContent();
    }

    // Helper method to map Entity -> DTO
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
