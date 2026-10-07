using Infrastructure.BackgroundServices;
using Infrastructure.Configuration;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MarketWizard.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class ServiceRegistration
{
     public static TBuilder AddInfrastructureServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
     {
          builder.AddNpgsqlDbContext<MarketWizardContext>("marketwizard");

          builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
          builder.Services.AddScoped<IMiscRepository, SqlMiscRepository>();
          builder.Services.AddScoped<ICronJobRepository, CronJobRepository>();
          builder.Services.AddScoped<IMarketDataProvider, MockMarketDataProvider>();

          builder.Services.Configure<MarketDataSyncOptions>(builder.Configuration.GetSection(MarketDataSyncOptions.SectionName));
          builder.Services.AddHostedService<MarketDataSyncBackgroundService>();
          
          return builder;
     }
}