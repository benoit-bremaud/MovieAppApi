using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MovieAppApi.Src.Controllers;
using MovieAppApi.Src.Core.Exceptions;
using MovieAppApi.Src.Models.Movie;
using MovieAppApi.Src.Core.Services.Movie;
using Xunit;

namespace MovieAppApi.Tests;

public class MoviesControllerTests
{
    private readonly Mock<IMovieService> _movieServiceMock;
    private readonly Mock<ILogger<MoviesController>> _loggerMock;
    private readonly MoviesController _controller;

    public MoviesControllerTests()
    {
        _movieServiceMock = new Mock<IMovieService>();
        _loggerMock = new Mock<ILogger<MoviesController>>();
        _controller = new MoviesController(_movieServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetMovieById_ReturnsOk_WhenMovieExists()
    {
        // Arrange
        int movieId = 123;
        var expectedMovie = new MovieModel(
            id: movieId,
            title: "Inception",
            originalTitle: "Inception",
            overview: "A mind-bending thriller",
            popularity: 8.5,
            releaseDate: new DateTime(2010, 7, 16),
            posterPath: "/path/to/poster.jpg",
            voteAverage: 8.8,
            voteCount: 25000,
            originalLanguage: "en"
        );
        
        _movieServiceMock.Setup(s => s.GetMovieByIdAsync(movieId, "en"))
            .ReturnsAsync(expectedMovie);

        // Act
        var result = await _controller.GetMovieById(movieId, "en");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMovie = Assert.IsType<MovieModel>(okResult.Value);
        Assert.Equal("Inception", returnedMovie.Title);
    }

    [Fact]
    public async Task GetMovieById_ReturnsNotFound_WhenServiceThrowsMovieNotFound()
    {
        // Arrange
        int movieId = 999;
        
        _movieServiceMock.Setup(s => s.GetMovieByIdAsync(movieId, "en"))
            .ThrowsAsync(new MovieNotFoundException("Movie not found"));

        // Act
        var result = await _controller.GetMovieById(movieId, "en");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
    }
}
