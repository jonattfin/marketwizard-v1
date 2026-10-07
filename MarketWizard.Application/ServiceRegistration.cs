using FluentValidation;
using MarketWizard.Application.Behaviors;
using MarketWizard.Application.Interfaces;
using MarketWizard.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MarketWizard.Application;

public static class ServiceRegistration
{
     public static IServiceCollection AddApplicationServices(this IServiceCollection services)
     {
          services.AddMediatR(cfg =>
          {
              cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly);
              cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
              cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
          });

          services.AddValidatorsFromAssembly(typeof(ServiceRegistration).Assembly);

          services.AddScoped<IMarketDataSyncService, MarketDataSyncService>();
          
          return services;
     }
}