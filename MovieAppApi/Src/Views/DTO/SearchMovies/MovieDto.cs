using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Src.Views.DTO.SearchMovies;

/// <summary>
/// Data Transfer Object for representing a movie in API responses
/// </summary>
public class MovieDto
{
    /// <summary>
    /// Movie ID
    /// </summary>
    [Required]
    public required int id { get; init; }

    /// <summary>
    /// Original language code
    /// </summary>
    [Required]
    public required string original_language { get; init; }

    /// <summary>
    /// Original title
    /// </summary>
    [Required]
    public required string original_title { get; init; }

    /// <summary>
    /// Movie description
    /// </summary>
    [Required]
    public required string overview { get; init; }

    /// <summary>
    /// Popularity score
    /// </summary>
    [Required]
    public required double popularity { get; init; }

    /// <summary>
    /// Localized title
    /// </summary>
    [Required]
    public required string title { get; init; }

    /// <summary>
    /// Average vote score (0-10)
    /// </summary>
    [Required]
    public required double vote_average { get; init; }

    /// <summary>
    /// Total votes count
    /// </summary>
    [Required]
    public required int vote_count { get; init; }

    /// <summary>
    /// Release date (nullable)
    /// </summary>
    public DateTime? release_date { get; init; }

    /// <summary>
    /// Poster image path (nullable)
    /// </summary>
    public string? poster_path { get; init; }
}
