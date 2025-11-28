using MovieAppApi.Src.Models.Movie;
using MovieAppApi.Src.Models.SearchMovies;

namespace MovieAppApi.Src.Core.Services.Movie;

/// <summary>
/// Service interface for movie-related business logic
/// </summary>
public interface IMovieService
{
    /// <summary>
    /// Searches for movies and returns results
    /// </summary>
    /// <param name="query">Search query with term and language</param>
    /// <returns>Search results</returns>
    Task<SearchMoviesResultModel> SearchMoviesAsync(SearchMoviesRequestQueryModel query);

    /// <summary>
    /// Gets movie details by TMDB ID
    /// </summary>
    Task<MovieModel> GetMovieByIdAsync(int tmdbId, string language = "en");
}

