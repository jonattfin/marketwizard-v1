using MarketWizard.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MarketWizard.Application;

public static class ServiceRegistration
{
     public static IServiceCollection AddApplicationServices(this IServiceCollection services)
     {
          services.AddSingleton<IMiscService, MiscService>();
          services.AddSingleton<IWatchlistService, WatchlistService>();
          
          return services;
     }
}