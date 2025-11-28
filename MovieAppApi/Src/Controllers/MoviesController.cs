using MovieAppApi.Src.Core.Services.Movie;
using MovieAppApi.Src.Models.SearchMovies;
using MovieAppApi.Src.Views.DTO.SearchMovies;
using Microsoft.AspNetCore.Mvc;
using MovieAppApi.Src.Core.Exceptions;

namespace MovieAppApi.Src.Controllers;

/// <summary>
/// Controller for searching and retrieving movie information from the TMDB API.
/// Provides endpoints for searching movies by title and retrieving detailed movie information by ID.
/// </summary>
public class MoviesController : BaseController<MoviesController>
{
    private readonly IMovieService _movieService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MoviesController"/> class.
    /// </summary>
    /// <param name="movieService">The service for fetching movie data from TMDB.</param>
    /// <param name="logger">The logger instance for recording controller operations.</param>
    public MoviesController(IMovieService movieService, ILogger<MoviesController> logger) : base(logger)
    {
        _movieService = movieService;
    }

    /// <summary>
    /// Searches for movies by title and optional language filter.
    /// </summary>
    /// <param name="query">The search query containing the title and preferred language.</param>
    /// <returns>An <see cref="OkObjectResult"/> with a paginated list of matching movies.</returns>
    /// <remarks>
    /// Returns a paginated result set with detailed movie information including popularity, ratings, and release dates.
    /// Language parameter follows ISO 639-1 format (e.g., "en", "fr", "es").
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Search([FromQuery] SearchMoviesRequestQueryDto query)
    {
        Logger.LogInformation(
            "Search movies endpoint called - SearchTerm: {SearchTerm}, Language: {Language}",
            query.search_term,
            query.language
        );

        try
        {
            var requestModel = new SearchMoviesRequestQueryModel(query.search_term, query.language);
            var result = await _movieService.SearchMoviesAsync(requestModel);
            var response = new SearchMoviesResponseDto
            {
                page = result.Page,
                results = result.Results.Select(m => new MovieDto
                {
                    id = m.Id,
                    original_language = m.OriginalLanguage,
                    original_title = m.OriginalTitle,
                    overview = m.Overview,
                    popularity = m.Popularity,
                    title = m.Title,
                    vote_average = m.VoteAverage,
                    vote_count = m.VoteCount,
                    release_date = m.ReleaseDate,
                    poster_path = m.PosterPath
                }).ToList(),
                total_pages = result.TotalPages,
                total_results = result.TotalResults
            };

            Logger.LogInformation("Search successful - Returning {MovieCount} movies", response.results.Count);
            return Ok(response);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during movie search");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while searching for movies" });
        }
    }

    /// <summary>
    /// Retrieves detailed information about a specific movie by its TMDB ID.
    /// </summary>
    /// <param name="id">The TMDB ID of the movie to retrieve.</param>
    /// <param name="language">The preferred language for the response (ISO 639-1 format). Default is "en".</param>
    /// <returns>An <see cref="OkObjectResult"/> containing detailed movie information if found.</returns>
    /// <exception cref="MovieNotFoundException">Thrown when the specified movie ID is not found.</exception>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovieById(int id, [FromQuery] string language = "en")
    {
        Logger.LogInformation("Getting movie details - TmdbId: {MovieId}, Language: {Language}", id, language);

        try
        {
            var movie = await _movieService.GetMovieByIdAsync(id, language);
            Logger.LogInformation("Retrieved movie {MovieTitle}", movie.Title);
            return Ok(movie);
        }
        catch (MovieNotFoundException ex)
        {
            Logger.LogWarning("Movie {MovieId} not found", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving movie {MovieId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error retrieving movie" });
        }
    }
}
