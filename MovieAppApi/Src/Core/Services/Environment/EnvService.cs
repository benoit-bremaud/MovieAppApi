using DotNetEnv;

namespace MovieAppApi.Src.Core.Services.Environment;

/// <summary>
/// Implementation of <see cref="IEnvService"/> that loads and manages environment variables from .env file or system environment.
/// Provides secure access to configuration values like TMDB API credentials and application environment settings.
/// </summary>
public class EnvService : IEnvService
{
    private readonly ILogger<EnvService> _logger;

    /// <summary>
    /// Gets the TMDB API key loaded from environment configuration.
    /// This key is required for authenticating with The Movie Database (TMDB) API.
    /// </summary>
    public string TmdbApiKey { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the current application environment (e.g., "Development", "Staging", "Production").
    /// Used to configure environment-specific behavior and logging levels.
    /// </summary>
    public string Environment { get; private set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvService"/> class.
    /// Automatically loads configuration from .env file or system environment variables on instantiation.
    /// </summary>
    /// <param name="logger">The logger instance for recording configuration loading operations.</param>
    public EnvService(ILogger<EnvService> logger)
    {
        _logger = logger;
        LoadConfiguration();
    }

    /// <summary>
    /// Loads environment variables from .env file or system environment.
    /// Validates required configuration values and logs the initialization process.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when required environment variables are missing.</exception>
    private void LoadConfiguration()
    {
        _logger.LogInformation("🚀 Initializing environment service...");

        try
        {
            // Load .env file from project root (one level up from MovieAppApi/)
            string envPath = Path.Combine(Directory.GetCurrentDirectory(), "../.env");
            if (File.Exists(envPath))
            {
                _logger.LogInformation("📂 Loading .env file from: {Path}", envPath);
                DotNetEnv.Env.Load(envPath);
            }
            else
            {
                _logger.LogWarning("⚠️ .env file not found at: {Path}. Using system environment variables.", envPath);
            }

            // Load and validate environment variables
            TmdbApiKey = GetVariable("TMDB_API_KEY");
            Environment = GetVariable("ASPNETCORE_ENVIRONMENT", false) ?? "Production";
            _logger.LogInformation("✅ Environment loaded successfully: {Env}", Environment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Critical error loading configuration");
            throw; // Block startup if configuration is invalid
        }
    }

    /// <summary>
    /// Retrieves an environment variable by key with optional requirement enforcement.
    /// Safely logs variable values while masking sensitive information (API keys, secrets).
    /// </summary>
    /// <param name="key">The name of the environment variable to retrieve.</param>
    /// <param name="required">Whether the variable is required. Throws exception if required and missing.</param>
    /// <returns>The value of the environment variable, or empty string if optional and not found.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a required variable is missing.</exception>
    private string GetVariable(string key, bool required = true)
    {
        var value = System.Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrWhiteSpace(value))
        {
            if (required)
            {
                _logger.LogCritical("❌ Required environment variable missing: {Key}", key);
                throw new InvalidOperationException($"Environment variable '{key}' is required but not set.");
            }

            return string.Empty;
        }

        // Do not log API keys or secrets for security
        if (key.Contains("KEY") || key.Contains("SECRET"))
            _logger.LogDebug("🔑 Variable loaded: {Key} = ********", key);
        else
            _logger.LogDebug("Variable loaded: {Key} = {Value}", key, value);

        return value;
    }
}
