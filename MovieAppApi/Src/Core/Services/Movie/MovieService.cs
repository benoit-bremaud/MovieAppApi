using MovieAppApi.Src.Core.Services.FetchMovies;
using MovieAppApi.Src.Models.Movie;
using MovieAppApi.Src.Models.SearchMovies;

namespace MovieAppApi.Src.Core.Services.Movie;

/// <summary>
/// Implementation of <see cref="IMovieService"/> that orchestrates movie search and retrieval operations.
/// Acts as a facade layer between controllers and the underlying TMDB fetch service,
/// providing centralized logging and error handling for movie-related business logic.
/// </summary>
public class MovieService : IMovieService
{
    private readonly IFetchMoviesService _fetchMoviesService;
    private readonly ILogger<MovieService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieService"/> class.
    /// </summary>
    /// <param name="fetchMoviesService">The service responsible for fetching movie data from external APIs.</param>
    /// <param name="logger">The logger instance for recording service operations.</param>
    public MovieService(IFetchMoviesService fetchMoviesService, ILogger<MovieService> logger)
    {
        _fetchMoviesService = fetchMoviesService;
        _logger = logger;
    }

    /// <summary>
    /// Searches for movies based on the provided query parameters.
    /// </summary>
    /// <param name="query">The search query containing title and language preferences.</param>
    /// <returns>A <see cref="SearchMoviesResultModel"/> with paginated search results.</returns>
    /// <remarks>
    /// Logs comprehensive information about the search process including total results, current page, and total pages.
    /// Propagates exceptions from the underlying fetch service for proper error handling in the controller layer.
    /// </remarks>
    public async Task<SearchMoviesResultModel> SearchMoviesAsync(SearchMoviesRequestQueryModel query)
    {
        _logger.LogInformation(
            "🎬 MovieService: Starting search - SearchTerm: {SearchTerm}, Language: {Language}",
            query.SearchTerm,
            query.Language
        );

        try
        {
            var results = await _fetchMoviesService.SearchMoviesAsync(query);
            _logger.LogInformation(
                "✅ MovieService: Search completed successfully - {TotalResults} total results, page {Page}/{TotalPages}",
                results.TotalResults,
                results.Page,
                results.TotalPages
            );
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ MovieService: Error during search");
            throw;
        }
    }

    /// <summary>
    /// Retrieves detailed information about a specific movie by its TMDB ID.
    /// </summary>
    /// <param name="tmdbId">The TMDB ID of the movie to retrieve.</param>
    /// <param name="language">The preferred language for the response (ISO 639-1 format). Default is "en".</param>
    /// <returns>A <see cref="MovieModel"/> containing detailed movie information.</returns>
    public async Task<MovieModel> GetMovieByIdAsync(int tmdbId, string language = "en")
    {
        _logger.LogInformation("Getting movie details - TmdbId: {TmdbId}, Language: {Language}", tmdbId, language);
        return await _fetchMoviesService.GetMovieByIdAsync(tmdbId, language);
    }
}
