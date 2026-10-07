using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using MarketWizard.Server.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MarketWizard.Infrastructure.UnitTests;

public class MiddlewaresUnitTest
{
    [Fact]
    public async Task RequestLoggingMiddleware_LogsStartAndFinish()
    {
        var logger = Substitute.For<ILogger<RequestLoggingMiddleware>>();
        var middleware = new RequestLoggingMiddleware(
            context =>
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Task.CompletedTask;
            },
            logger);

        var context = new DefaultHttpContext();
        context.Request.Path = "/api/watchlist";
        context.Request.Method = "GET";

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        logger.ReceivedWithAnyArgs(2).Log(
            LogLevel.Information,
            default,
            Arg.Any<object>(),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task RequestLoggingMiddleware_WhenExceptionThrown_LogsErrorAndRethrows()
    {
        var logger = Substitute.For<ILogger<RequestLoggingMiddleware>>();
        var expectedException = new InvalidOperationException("DB error");
        var middleware = new RequestLoggingMiddleware(
            _ => Task.FromException(expectedException),
            logger);

        var context = new DefaultHttpContext();
        context.Request.Path = "/api/watchlist";
        context.Request.Method = "GET";

        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(
            () => middleware.InvokeAsync(context));

        Assert.Same(expectedException, actualException);
        logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            expectedException,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task ValidationExceptionMiddleware_WhenNoException_PassesThrough()
    {
        var logger = Substitute.For<ILogger<ValidationExceptionMiddleware>>();
        var called = false;
        var middleware = new ValidationExceptionMiddleware(
            context =>
            {
                called = true;
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Task.CompletedTask;
            },
            logger);

        var context = new DefaultHttpContext();
        await middleware.InvokeAsync(context);

        Assert.True(called);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task ValidationExceptionMiddleware_WhenValidationException_Returns400WithProblemDetails()
    {
        var logger = Substitute.For<ILogger<ValidationExceptionMiddleware>>();
        var failures = new List<ValidationFailure>
        {
            new("Name", "Name is required."),
            new("Id", "Id must be a valid GUID.")
        };
        var validationException = new ValidationException(failures);

        var middleware = new ValidationExceptionMiddleware(
            _ => Task.FromException(validationException),
            logger);

        var context = new DefaultHttpContext();
        var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await middleware.InvokeAsync(context);

        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Equal("application/problem+json", context.Response.ContentType);

        responseBody.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(responseBody);
        var json = await reader.ReadToEndAsync();

        var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.True(problemDetails.Errors.ContainsKey("Name"));
        Assert.True(problemDetails.Errors.ContainsKey("Id"));
    }
}
