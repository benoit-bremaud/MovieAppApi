using System.Diagnostics;
using System.Text.Json;
using MovieAppApi.Src.Core.Exceptions;
using MovieAppApi.Src.Core.Services.Environment;
using MovieAppApi.Src.Core.Services.Fetch;
using MovieAppApi.Src.Models.Movie;
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

    public async Task<MovieModel> GetMovieByIdAsync(int tmdbId, string language = "en")
    {
        _logger.LogInformation("🎬 Fetching movie details - TmdbId: {TmdbId}, Language: {Language}", tmdbId, language);

        try
        {
            var url = $"{_baseUrl}/movie/{tmdbId}?api_key={_apiKey}&language={language}";
            var startTime = DateTime.UtcNow;

            var response = await _httpClient.GetAsync(url);
            var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;

            _logger.LogInformation("📡 TMDB response received in {DurationMs}ms - StatusCode: {StatusCode}", duration, (int)response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                throw new TmdbApiException(
                    $"Failed to fetch movie details from TMDB: {response.StatusCode}", 
                    (int)response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();
            var movieDto = JsonSerializer.Deserialize<TmdbMovieDto>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new TmdbApiException("Failed to parse TMDB movie response");

            _logger.LogInformation("✅ Successfully parsed TMDB movie - Title: {Title}", movieDto.title);

            return movieDto.ToModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error fetching movie details - TmdbId: {TmdbId}", tmdbId);
            throw;
        }
    }


}
