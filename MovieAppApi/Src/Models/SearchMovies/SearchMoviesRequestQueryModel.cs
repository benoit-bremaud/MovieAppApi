namespace MovieAppApi.Src.Models.SearchMovies;

/// <summary>
/// Internal domain model for movie search query parameters
/// </summary>
public class SearchMoviesRequestQueryModel
{
    /// <summary>
    /// Search term to find movies
    /// </summary>
    public string SearchTerm { get; }

    /// <summary>
    /// Language code for results
    /// </summary>
    public string Language { get; }

    /// <summary>
    /// Constructor for creating a search query model
    /// </summary>
    public SearchMoviesRequestQueryModel(string searchTerm, string language)
    {
        SearchTerm = searchTerm;
        Language = language;
    }
}
