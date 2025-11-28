using MovieAppApi.Src.Core.Database;
using MovieAppApi.Src.Core.Services.Environment;
using MovieAppApi.Src.Core.Services.FetchMovies;
using MovieAppApi.Src.Core.Services.FetchMovies.Tmdb;
using MovieAppApi.Src.Core.Services.Movie;
using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Core.Repositories;
using MovieAppApi.Src.Middlewares;


namespace MovieAppApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure logging
        builder.Services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
            logging.SetMinimumLevel(LogLevel.Information);
        });

        // Register environment service
        builder.Services.AddSingleton<IEnvService, EnvService>();

        // Register HTTP client for TMDB
        builder.Services.AddHttpClient();

        // Register fetch and movie services
        builder.Services.AddScoped<IFetchMoviesService, TmdbService>();
        builder.Services.AddScoped<IMovieService, MovieService>();

        // Register Database Context (SQLite)
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=movieapp.db"));

        // Register Repositories
        builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();

        // Add controllers and Swagger
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            options.AddPolicy("AllowLocalhost", builder =>
            {
                builder.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:5173",
                    "http://localhost:5174",
                    "http://localhost:8080")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        // Verify environment configuration on startup
        using (var scope = app.Services.CreateScope())
        {
            var envService = scope.ServiceProvider.GetRequiredService<IEnvService>();
            // Logging happens automatically in EnvService constructor
        }

        // Configure HTTP pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowLocalhost");

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();

        app.Run();
    }
}
