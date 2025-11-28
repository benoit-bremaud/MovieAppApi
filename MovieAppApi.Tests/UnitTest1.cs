using Xunit;
using MovieAppApi.Src.Core.Exceptions;

namespace MovieAppApi.Tests;

/// <summary>
/// Basic unit tests to verify project structure and exception handling
/// </summary>
public class ExceptionHandlingTests
{
    [Fact]
    public void MovieNotFoundException_ShouldBeInstantiable()
    {
        // Act
        var exception = new MovieNotFoundException("Test message");

        // Assert
        Assert.NotNull(exception);
        Assert.Equal("Test message", exception.Message);
    }

    [Fact]
    public void PlaylistNotFoundException_ShouldBeInstantiable()
    {
        // Act
        var exception = new PlaylistNotFoundException("Test message");

        // Assert
        Assert.NotNull(exception);
        Assert.Equal("Test message", exception.Message);
    }

    [Fact]
    public void TmdbApiException_ShouldStoreStatusCode()
    {
        // Act
        var exception = new TmdbApiException("API Error", 503);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(503, exception.StatusCode);
        Assert.Equal("API Error", exception.Message);
    }
}
