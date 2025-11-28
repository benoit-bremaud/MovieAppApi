using MovieAppApi.Src.Models.Movie;

namespace MovieAppApi.Src.Models.SearchMovies;

/// <summary>
/// Internal model representing the result of a movie search
/// </summary>
public class SearchMoviesResultModel
{
    /// <summary>
    /// Current page number
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// List of movies found in this search
    /// </summary>
    public List<MovieModel> Results { get; }

    /// <summary>
    /// Total number of pages available
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Total number of results matching the search
    /// </summary>
    public int TotalResults { get; }

    /// <summary>
    /// Constructor for creating a SearchMoviesResultModel instance
    /// </summary>
    public SearchMoviesResultModel(int page, List<MovieModel> results, int totalPages, int totalResults)
    {
        Page = page;
        Results = results;
        TotalPages = totalPages;
        TotalResults = totalResults;
    }
}
