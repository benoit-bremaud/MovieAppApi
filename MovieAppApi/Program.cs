using MovieAppApi.Src.Core.Services.Environment;

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

        // Add controllers and Swagger
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
