using Gateways;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Domain;

public static class ServiceCollectionExtensions
{    
    public static IServiceCollection AddDomain(this IServiceCollection services, Assembly domainAssembly = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) =>
        services
            .AddServices(typeof(IUseCase<>), domainAssembly, serviceLifetime)
            .AddScoped<IInputHandler, InputHandler>();

    public static IServiceCollection AddServices(this IServiceCollection services, Type serviceInterfaceType, Assembly scanAssembly = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        scanAssembly ??= Assembly.GetCallingAssembly();

        foreach (var implementationType in scanAssembly.GetImplementations(serviceInterfaceType))
        {
            foreach (var serviceType in implementationType.GetInterfaces())
            {
                services.Add(new ServiceDescriptor(serviceType, implementationType, serviceLifetime));
            }
        }

        return services;
    }

    public static IServiceCollection AddGateways(this IServiceCollection services, UpdaterConfiguration configuration) =>
        services
            .AddServices(typeof(ISpoolmanEndpoint<>), typeof(SpoolmanClient).Assembly, ServiceLifetime.Scoped)
            .AddSingleton(configuration.Spoolman)
            .AddSingleton(configuration.HomeAssistant)
            .AddScoped<HomeAssistantClient>()
            .AddScoped<SpoolmanClient>()
            .AddScoped<GatewayChecker>()
            .AddTransient<LoggingHandler>();
}
