namespace MovieAppApi.Src.Models.Movie;

/// <summary>
/// Internal domain model representing a movie
/// </summary>
public class MovieModel
{
    /// <summary>
    /// Unique identifier for the movie
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Original language code (e.g., "en", "fr")
    /// </summary>
    public string OriginalLanguage { get; }

    /// <summary>
    /// Original title in the source language
    /// </summary>
    public string OriginalTitle { get; }

    /// <summary>
    /// Movie description or summary
    /// </summary>
    public string Overview { get; }

    /// <summary>
    /// Popularity score from TMDB
    /// </summary>
    public double Popularity { get; }

    /// <summary>
    /// URL path to the poster image (nullable)
    /// </summary>
    public string? PosterPath { get; }

    /// <summary>
    /// Movie release date (nullable)
    /// </summary>
    public DateTime? ReleaseDate { get; }

    /// <summary>
    /// Localized title for the specified language
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Average vote score (0.0 to 10.0)
    /// </summary>
    public double VoteAverage { get; }

    /// <summary>
    /// Total number of votes received
    /// </summary>
    public int VoteCount { get; }

    /// <summary>
    /// Constructor for creating a MovieModel instance
    /// </summary>
    public MovieModel(
        int id,
        string originalLanguage,
        string originalTitle,
        string overview,
        double popularity,
        DateTime? releaseDate,
        string title,
        double voteAverage,
        int voteCount,
        string? posterPath)
    {
        Id = id;
        OriginalLanguage = originalLanguage;
        OriginalTitle = originalTitle;
        Overview = overview;
        Popularity = popularity;
        ReleaseDate = releaseDate;
        Title = title;
        VoteAverage = voteAverage;
        VoteCount = voteCount;
        PosterPath = posterPath;
    }
}
