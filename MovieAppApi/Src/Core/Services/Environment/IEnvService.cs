namespace MovieAppApi.Src.Core.Services.Environment;

/// <summary>
/// Service responsible for managing environment variables
/// </summary>
public interface IEnvService
{
    /// <summary>
    /// Gets the TMDB API key
    /// </summary>
    string TmdbApiKey { get; }
    
    /// <summary>
    /// Gets the current environment (Development, Production...)
    /// </summary>
    string Environment { get; }
}
