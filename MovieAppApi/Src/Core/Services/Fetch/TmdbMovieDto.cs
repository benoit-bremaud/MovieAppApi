using System.ComponentModel.DataAnnotations;
using MovieAppApi.Src.Models.Movie;

namespace MovieAppApi.Src.Core.Services.Fetch;

/// <summary>
/// Data Transfer Object representing a movie as received from TMDB API
/// </summary>
public class TmdbMovieDto
{
    /// <summary>
    /// Movie ID from TMDB
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
    /// Movie overview (can be empty string)
    /// </summary>
    [Required(AllowEmptyStrings = true)]
    public required string overview { get; init; }

    /// <summary>
    /// Popularity metric
    /// </summary>
    [Required]
    public required double popularity { get; init; }

    /// <summary>
    /// Release date (can be empty string)
    /// </summary>
    [Required(AllowEmptyStrings = true)]
    public required string release_date { get; init; }

    /// <summary>
    /// Localized title
    /// </summary>
    [Required]
    public required string title { get; init; }

    /// <summary>
    /// Vote average (0-10)
    /// </summary>
    [Required]
    public required double vote_average { get; init; }

    /// <summary>
    /// Number of votes
    /// </summary>
    [Required]
    public required int vote_count { get; init; }

    /// <summary>
    /// Poster image path (nullable)
    /// </summary>
    public string? poster_path { get; init; }

    /// <summary>
    /// Converts this TMDB DTO to an internal MovieModel
    /// </summary>
    /// <returns>MovieModel instance with validated data</returns>
    /// <exception cref="ValidationException">Thrown if DTO data is invalid</exception>
    public MovieModel ToModel()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);

        return new MovieModel(
            id: id,
            originalLanguage: original_language,
            originalTitle: original_title,
            overview: overview,
            popularity: popularity,
            releaseDate: string.IsNullOrEmpty(release_date) ? null : DateTime.Parse(release_date),
            title: title,
            voteAverage: vote_average,
            voteCount: vote_count,
            posterPath: poster_path
        );
    }
}
