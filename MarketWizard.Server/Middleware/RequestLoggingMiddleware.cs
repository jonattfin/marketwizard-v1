using System.Diagnostics;

namespace MarketWizard.Server.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;
        var method = context.Request.Method;
        var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;

        logger.LogInformation("HTTP {Method} {Path}{QueryString} started", method, path, queryString);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context);
            stopwatch.Stop();

            logger.LogInformation(
                "HTTP {Method} {Path} finished with status {StatusCode} in {ElapsedMilliseconds}ms",
                method,
                path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(
                ex,
                "HTTP {Method} {Path} failed with unhandled exception after {ElapsedMilliseconds}ms",
                method,
                path,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
