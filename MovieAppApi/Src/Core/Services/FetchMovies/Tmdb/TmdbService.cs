using System.Diagnostics;
using MovieAppApi.Src.Core.Services.Environment;
using MovieAppApi.Src.Core.Services.Fetch;
using MovieAppApi.Src.Models.SearchMovies;

namespace MovieAppApi.Src.Core.Services.FetchMovies.Tmdb;

public class TmdbService : IFetchMoviesService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TmdbService> _logger;
    private readonly string _apiKey;
    private readonly string _baseUrl = "https://api.themoviedb.org/3";

    public TmdbService(HttpClient httpClient, IEnvService envService, ILogger<TmdbService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = envService.TmdbApiKey;
    }

    public async Task<SearchMoviesResultModel> SearchMoviesAsync(SearchMoviesRequestQueryModel query)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Starting TMDB search - SearchTerm: {SearchTerm}, Language: {Language}",
            query.SearchTerm,
            query.Language
        );

        try
        {
            var url = $"{_baseUrl}/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(query.SearchTerm)}&language={query.Language}";

            _logger.LogDebug("Calling TMDB API");

            var response = await _httpClient.GetAsync(url);
            stopwatch.Stop();

            _logger.LogInformation(
                "TMDB response received in {ElapsedMilliseconds}ms - StatusCode: {StatusCode}",
                stopwatch.ElapsedMilliseconds,
                response.StatusCode
            );

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "TMDB API returned error - StatusCode: {StatusCode}",
                    response.StatusCode
                );
                throw new HttpRequestException($"TMDB API returned {response.StatusCode}");
            }

            var dto = await response.Content.ReadFromJsonAsync<TmdbSearchMoviesResultDto>();
            if (dto == null)
            {
                _logger.LogError("Failed to deserialize TMDB response");
                throw new NullReferenceException("TMDB response deserialization returned null");
            }

            _logger.LogInformation(
                "Successfully parsed TMDB response - Found {MovieCount} movies",
                dto.results.Count
            );

            var result = dto.ToModel();
            _logger.LogDebug("Converted TMDB DTO to SearchMoviesResultModel");

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error during TMDB search");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during TMDB search");
            throw;
        }
    }
}
