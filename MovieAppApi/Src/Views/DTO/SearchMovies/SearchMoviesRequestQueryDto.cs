using System.ComponentModel.DataAnnotations;

namespace MovieAppApi.Src.Views.DTO.SearchMovies;

/// <summary>
/// Data Transfer Object for movie search request query parameters
/// </summary>
public class SearchMoviesRequestQueryDto
{
    /// <summary>
    /// Search term to find movies (e.g., "marvel", "inception")
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Search term is required")]
    public required string search_term { get; init; }

    /// <summary>
    /// Language code for the search results (e.g., "en", "fr")
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Language is required")]
    [AllowedValues("en", "fr", ErrorMessage = "Language must be one of: en, fr")]
    public required string language { get; init; }
}
