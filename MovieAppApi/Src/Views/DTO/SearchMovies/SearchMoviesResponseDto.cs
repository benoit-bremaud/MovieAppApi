using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Src.Views.DTO.SearchMovies;

/// <summary>
/// Data Transfer Object for movie search API response
/// </summary>
public class SearchMoviesResponseDto
{
    /// <summary>
    /// Current page number of results
    /// </summary>
    [Required]
    public required int page { get; init; }

    /// <summary>
    /// List of movies in this page
    /// </summary>
    [Required]
    public required List<MovieDto> results { get; init; }

    /// <summary>
    /// Total number of pages available
    /// </summary>
    [Required]
    public required int total_pages { get; init; }

    /// <summary>
    /// Total number of results matching the search
    /// </summary>
    [Required]
    public required int total_results { get; init; }
}
