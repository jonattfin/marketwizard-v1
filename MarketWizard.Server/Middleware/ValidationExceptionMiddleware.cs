using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace MarketWizard.Server.Middleware;

public class ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation failure occurred for request {Path}", context.Request.Path);

            var errors = ex.Errors
                .GroupBy(e => string.IsNullOrWhiteSpace(e.PropertyName) ? "General" : e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            var problemDetails = new ValidationProblemDetails(errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details.",
                Instance = context.Request.Path
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, JsonOptions));
        }
    }
}
