using Infrastructure.Repositories;
using MarketWizard.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceRegistration
{
     public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
     {
          services.AddSingleton<IWatchlistRepository, WatchlistRepository>();
          services.AddSingleton<IMiscRepository, MiscRepository>();
          
          return services;
     }
}