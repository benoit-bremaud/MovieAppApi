using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using MovieAppApi.Src.Core.Exceptions;
using MovieAppApi.Src.Middlewares;
using Xunit;

namespace MovieAppApi.Tests;

public class MiddlewareTests
{
    [Fact]
    public async Task Middleware_Returns404_ForMovieNotFoundException()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();

        // Set up a memory stream to capture the response body
        var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        // Simulate a next delegate that throws MovieNotFoundException
        RequestDelegate next = (ctx) => throw new MovieNotFoundException("Movie not found!");

        var middleware = new ExceptionHandlingMiddleware(next, loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);

        responseBody.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(responseBody);
        var bodyString = await reader.ReadToEndAsync();

        var json = JsonDocument.Parse(bodyString).RootElement;

        Assert.Equal("Movie not found", json.GetProperty("message").GetString());
        Assert.Equal("Movie not found!", json.GetProperty("details").GetString());
        Assert.StartsWith("application/json", context.Response.ContentType);
    }

}
