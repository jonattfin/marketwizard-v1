using System.Linq.Expressions;
using System.Text.Json;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Entities;
using MarketWizard.Application.Interfaces;
using MarketWizard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SqlMiscRepository(MarketWizardContext context) : IMiscRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Task<IndicePerformanceData> GetIndices() =>
        GetLatestDataAsync<IndicePerformance, IndicePerformanceData>(c => new Snapshot(c.Date, c.IndicePerfomance));

    public Task<TopNewsData> GetTopNews() =>
        GetLatestDataAsync<TopNews, TopNewsData>(c => new Snapshot(c.Date, c.TopNews));

    public Task<SectorPerformanceData> GetSectorPerformance() =>
        GetLatestDataAsync<SectorPerformance, SectorPerformanceData>(c => new Snapshot(c.Date, c.SectorPerformance));

    public Task<GainersData> GetTopGainers() =>
        GetLatestDataAsync<Gainers, GainersData>(c => new Snapshot(c.Date, c.Gainers));

    public Task<GainersData> GetTopLosers() =>
        GetLatestDataAsync<Gainers, GainersData>(c => new Snapshot(c.Date, c.Losers));

    public Task<GainersData> GetTopIndustries() =>
        GetLatestDataAsync<Gainers, GainersData>(c => new Snapshot(c.Date, c.TopIndustries));

    public Task<GainersData> GetWorstIndustries() =>
        GetLatestDataAsync<Gainers, GainersData>(c => new Snapshot(c.Date, c.WorstIndustries));

    private async Task<TData> GetLatestDataAsync<TItem, TData>(Expression<Func<CronJob, Snapshot>> projection)
        where TData : GenericData<TItem>, new()
    {
        var snapshot = await context.CronJobs
            .OrderByDescending(c => c.Date)
            .AsNoTracking()
            .Select(projection)
            .FirstOrDefaultAsync();

        return DeserializeData<TItem, TData>(snapshot?.Date, snapshot?.Json);
    }

    private static TData DeserializeData<TItem, TData>(DateTime? date, string? json)
        where TData : GenericData<TItem>, new()
    {
        var dateTimeOffset = date.HasValue
            ? new DateTimeOffset(DateTime.SpecifyKind(date.Value, DateTimeKind.Utc))
            : (DateTimeOffset?)null;

        if (string.IsNullOrWhiteSpace(json))
        {
            return new TData
            {
                Items = [],
                Date = dateTimeOffset ?? DateTimeOffset.UtcNow
            };
        }

        try
        {
            var trimmed = json.Trim();
            if (trimmed.StartsWith("["))
            {
                var items = JsonSerializer.Deserialize<List<TItem>>(trimmed, JsonOptions);
                return new TData
                {
                    Items = items ?? [],
                    Date = dateTimeOffset ?? DateTimeOffset.UtcNow
                };
            }

            var result = JsonSerializer.Deserialize<TData>(trimmed, JsonOptions);
            if (result != null)
            {
                result.Items ??= [];
                if (dateTimeOffset.HasValue && result.Date == null)
                {
                    result.Date = dateTimeOffset;
                }

                return result;
            }
        }
        catch (JsonException)
        {
            // In case of invalid JSON content, fallback to empty data
        }

        return new TData
        {
            Items = [],
            Date = dateTimeOffset ?? DateTimeOffset.UtcNow
        };
    }

    private sealed record Snapshot(DateTime Date, string? Json);
}