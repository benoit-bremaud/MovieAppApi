using MovieAppApi.Src.Models.SearchMovies;

namespace MovieAppApi.Src.Core.Services.FetchMovies;

/// <summary>
/// Service interface for fetching movies from external sources (e.g., TMDB)
/// </summary>
public interface IFetchMoviesService
{
    /// <summary>
    /// Searches for movies on TMDB based on search term and language
    /// </summary>
    /// <param name="query">Search query containing search term and language</param>
    /// <returns>Search results with list of movies</returns>
    Task<SearchMoviesResultModel> SearchMoviesAsync(SearchMoviesRequestQueryModel query);
}
