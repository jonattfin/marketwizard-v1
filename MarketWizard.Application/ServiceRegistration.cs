using MarketWizard.Application.Interfaces;
using MarketWizard.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MarketWizard.Application;

public static class ServiceRegistration
{
     public static IServiceCollection AddApplicationServices(this IServiceCollection services)
     {
          services.AddScoped<IMiscService, MiscService>();
          services.AddScoped<IWatchlistService, WatchlistService>();
          services.AddScoped<IMarketDataSyncService, MarketDataSyncService>();
          
          return services;
     }
}