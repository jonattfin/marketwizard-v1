namespace MarketWizard.Server.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app) =>
        app.UseMiddleware<RequestLoggingMiddleware>();

    public static IApplicationBuilder UseValidationExceptionHandler(this IApplicationBuilder app) =>
        app.UseMiddleware<ValidationExceptionMiddleware>();
}
