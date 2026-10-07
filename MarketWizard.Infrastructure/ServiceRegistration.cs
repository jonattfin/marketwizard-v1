using Infrastructure.Persistence;
using Infrastructure.Repositories;
using MarketWizard.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class ServiceRegistration
{
     public static TBuilder AddInfrastructureServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
     {
          builder.AddNpgsqlDbContext<MarketWizardContext>("marketwizard");

          builder.Services.AddSingleton<IWatchlistRepository, WatchlistRepository>();
          builder.Services.AddSingleton<IMiscRepository, MiscRepository>();
          
          return builder;
     }
}