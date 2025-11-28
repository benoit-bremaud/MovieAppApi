using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppApi.Src.Core.Database.Entities;

/// <summary>
/// Database entity representing a movie within a playlist
/// </summary>
[Table("PlaylistMovies")]
public class PlaylistMovieEntity
{
    /// <summary>
    /// Primary Key
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// TMDB Movie ID
    /// </summary>
    [Required]
    public int TmdbId { get; set; }

    /// <summary>
    /// Movie Title (cached for display)
    /// </summary>
    [Required]
    [MaxLength(200)]
    public required string Title { get; set; }

    /// <summary>
    /// Date added to playlist
    /// </summary>
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Foreign Key to Playlist
    /// </summary>
    [Required]
    public int PlaylistId { get; set; }

    /// <summary>
    /// Navigation property back to Playlist
    /// </summary>
    [ForeignKey(nameof(PlaylistId))]
    public PlaylistEntity? Playlist { get; set; }
}
