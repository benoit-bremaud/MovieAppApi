using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Core.Database.Entities;

namespace MovieAppApi.Src.Core.Database;

/// <summary>
/// Entity Framework Core database context for the MovieApp application.
/// Manages the database models, relationships, and migrations for playlists and associated movies.
/// Configured to use SQLite as the relational database provider.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The configuration options for the DbContext, including database connection details.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the collection of playlists in the database.
    /// </summary>
    public DbSet<PlaylistEntity> Playlists { get; set; }

    /// <summary>
    /// Gets or sets the collection of movies within playlists in the database.
    /// </summary>
    public DbSet<PlaylistMovieEntity> PlaylistMovies { get; set; }

    /// <summary>
    /// Configures the model relationships and database schema constraints.
    /// Defines the one-to-many relationship between playlists and movies with cascade delete behavior.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure entity relationships and constraints.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships if needed
        modelBuilder.Entity<PlaylistEntity>()
            .HasMany(p => p.Movies)
            .WithOne(m => m.Playlist)
            .HasForeignKey(m => m.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade); // If playlist is deleted, delete its movies
    }
}
