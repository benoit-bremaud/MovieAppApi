using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppApi.Src.Core.Database.Entities;

/// <summary>
/// Database entity representing a user playlist
/// </summary>
[Table("Playlists")]
public class PlaylistEntity
{
    /// <summary>
    /// Primary Key
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Playlist Name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Updated timestamp
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property for movies in this playlist
    /// </summary>
    public ICollection<PlaylistMovieEntity> Movies { get; set; } = new List<PlaylistMovieEntity>();
}
