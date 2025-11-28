using Microsoft.EntityFrameworkCore;
using MovieAppApi.Src.Core.Database.Entities;

namespace MovieAppApi.Src.Core.Database;

/// <summary>
/// Database context for the application
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Playlists table
    /// </summary>
    public DbSet<PlaylistEntity> Playlists { get; set; }

    /// <summary>
    /// PlaylistMovies table
    /// </summary>
    public DbSet<PlaylistMovieEntity> PlaylistMovies { get; set; }

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
