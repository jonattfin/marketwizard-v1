using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace MarketWizard.Server.Filters;

public class IdempotencyFilter : IEndpointFilter
{
    private readonly IDistributedCache _cache;
    private const string IdempotencyHeaderName = "X-Idempotency-Key";
    private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(2);

    public IdempotencyFilter(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;

        if (HttpMethods.IsGet(httpContext.Request.Method) || HttpMethods.IsHead(httpContext.Request.Method))
        {
            return await next(context);
        }

        if (!httpContext.Request.Headers.TryGetValue(IdempotencyHeaderName, out var headerValue) ||
            string.IsNullOrWhiteSpace(headerValue))
        {
            return await next(context);
        }

        var key = $"idempotency:{httpContext.Request.Path}:{headerValue}";

        var existingRecordJson = await _cache.GetStringAsync(key, httpContext.RequestAborted);
        if (!string.IsNullOrEmpty(existingRecordJson))
        {
            var cached = JsonSerializer.Deserialize<IdempotentResponse>(existingRecordJson);
            if (cached is not null)
            {
                if (cached.IsPending)
                {
                    return Results.Conflict(new { message = "A request with this idempotency key is currently processing." });
                }

                return Results.Content(
                    content: cached.ResponseBody,
                    contentType: cached.ContentType ?? "application/json",
                    statusCode: cached.StatusCode
                );
            }
        }

        var pendingRecord = new IdempotentResponse
        {
            IsPending = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _cache.SetStringAsync(
            key,
            JsonSerializer.Serialize(pendingRecord),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = DefaultTtl },
            httpContext.RequestAborted
        );

        try
        {
            var result = await next(context);
            await CacheExecutionResultAsync(key, result, httpContext);
            return result;
        }
        catch (Exception)
        {
            await _cache.RemoveAsync(key, CancellationToken.None);
            throw;
        }
    }

    private async Task CacheExecutionResultAsync(string cacheKey, object? result, HttpContext httpContext)
    {
        int statusCode = StatusCodes.Status200OK;
        string? responseBody = null;

        if (result is IStatusCodeHttpResult statusResult && statusResult.StatusCode.HasValue)
        {
            statusCode = statusResult.StatusCode.Value;
        }

        if (result is IValueHttpResult valueResult && valueResult.Value is not null)
        {
            responseBody = JsonSerializer.Serialize(valueResult.Value);
        }

        var completedRecord = new IdempotentResponse
        {
            IsPending = false,
            StatusCode = statusCode,
            ResponseBody = responseBody,
            ContentType = "application/json",
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(completedRecord),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = DefaultTtl },
            httpContext.RequestAborted
        );
    }
}

public class IdempotentResponse
{
    public bool IsPending { get; set; }
    public int StatusCode { get; set; }
    public string? ResponseBody { get; set; }
    public string? ContentType { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
