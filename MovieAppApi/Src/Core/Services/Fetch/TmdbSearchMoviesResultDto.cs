using System.ComponentModel.DataAnnotations;
using MovieAppApi.Src.Models.SearchMovies;

namespace MovieAppApi.Src.Core.Services.Fetch;

/// <summary>
/// Data Transfer Object representing search results as received from TMDB API
/// </summary>
public class TmdbSearchMoviesResultDto
{
    /// <summary>
    /// Current page number of results
    /// </summary>
    [Required]
    public required int page { get; init; }

    /// <summary>
    /// List of movies in this page of results
    /// </summary>
    [Required]
    public required List<TmdbMovieDto> results { get; init; }

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

    /// <summary>
    /// Converts this TMDB search result DTO to an internal SearchMoviesResultModel
    /// </summary>
    /// <returns>SearchMoviesResultModel with converted movies</returns>
    /// <exception cref="ValidationException">Thrown if DTO data is invalid</exception>
    public SearchMoviesResultModel ToModel()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);

        return new SearchMoviesResultModel(
            page: page,
            results: results.Select(movieDto => movieDto.ToModel()).ToList(),
            totalPages: total_pages,
            totalResults: total_results
        );
    }
}
