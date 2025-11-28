using MovieAppApi.Src.Core.Services.FetchMovies;
using MovieAppApi.Src.Models.Movie;
using MovieAppApi.Src.Models.SearchMovies;

namespace MovieAppApi.Src.Core.Services.Movie;

/// <summary>
/// Implementation of IMovieService - orchestrates movie search operations
/// </summary>
public class MovieService : IMovieService
{
    private readonly IFetchMoviesService _fetchMoviesService;
    private readonly ILogger<MovieService> _logger;

    /// <summary>
    /// Constructor for MovieService
    /// </summary>
    public MovieService(IFetchMoviesService fetchMoviesService, ILogger<MovieService> logger)
    {
        _fetchMoviesService = fetchMoviesService;
        _logger = logger;
    }

    /// <summary>
    /// Searches for movies using the fetch service
    /// </summary>
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

    public async Task<MovieModel> GetMovieByIdAsync(int tmdbId, string language = "en")
    {
        _logger.LogInformation("Getting movie details - TmdbId: {TmdbId}, Language: {Language}", tmdbId, language);
        return await _fetchMoviesService.GetMovieByIdAsync(tmdbId, language);
    }

}
