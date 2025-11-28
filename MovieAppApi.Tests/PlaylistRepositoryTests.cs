using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MovieAppApi.Src.Core.Database;
using MovieAppApi.Src.Core.Database.Entities;
using MovieAppApi.Src.Core.Repositories;
using Xunit;

namespace MovieAppApi.Tests;

public class PlaylistRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly PlaylistRepository _repository;
    
    public PlaylistRepositoryTests()
    {
        // Utiliser une DB In-Memory SQLite unique pour chaque test pour l'isolation
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        _context = new AppDbContext(options);
        _context.Database.OpenConnection(); // SQLite in-memory a besoin d'une connexion ouverte pour persister les tables
        _context.Database.EnsureCreated();

        var loggerMock = new Mock<ILogger<PlaylistRepository>>();
        _repository = new PlaylistRepository(_context, loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddPlaylistToDatabase()
    {
        // Arrange
        var newPlaylist = new PlaylistEntity { Name = "My Action Movies" };

        // Act
        var created = await _repository.CreateAsync(newPlaylist);

        // Assert
        Assert.NotEqual(0, created.Id); // L'ID doit être généré
        Assert.Equal("My Action Movies", created.Name);

        // Vérifier que c'est bien en base
        var inDb = await _context.Playlists.FindAsync(created.Id);
        Assert.NotNull(inDb);
        Assert.Equal("My Action Movies", inDb.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateNameAndTimestamp()
    {
        // Arrange : Créer d'abord une playlist
        var playlist = new PlaylistEntity { Name = "Original Name" };
        await _context.Playlists.AddAsync(playlist);
        await _context.SaveChangesAsync();

        var oldUpdatedAt = playlist.UpdatedAt;
        
        // Petit délai pour garantir que le timestamp change
        await Task.Delay(100);

        // Act
        var updated = await _repository.UpdateAsync(playlist.Id, "Updated Name");

        // Assert
        Assert.Equal("Updated Name", updated.Name);
        Assert.True(updated.UpdatedAt > oldUpdatedAt, "UpdatedAt timestamp should be newer");

        // Vérif en base
        var inDb = await _context.Playlists.FindAsync(playlist.Id);
        Assert.Equal("Updated Name", inDb!.Name);
    }
    
    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }
}
