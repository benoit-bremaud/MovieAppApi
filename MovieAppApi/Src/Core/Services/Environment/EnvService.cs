using DotNetEnv;

namespace MovieAppApi.Src.Core.Services.Environment;

/// <summary>
/// Implementation of IEnvService that loads environment variables from .env file
/// </summary>
public class EnvService : IEnvService
{
    private readonly ILogger<EnvService> _logger;
    
    public string TmdbApiKey { get; private set; } = string.Empty;
    public string Environment { get; private set; } = string.Empty;

    public EnvService(ILogger<EnvService> logger)
    {
        _logger = logger;
        LoadConfiguration();
    }

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
