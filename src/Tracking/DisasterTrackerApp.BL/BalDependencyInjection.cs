using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.BL.HttpClients.Implementation;
using DisasterTrackerApp.BL.Implementation;
using DisasterTrackerApp.BL.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DisasterTrackerApp.BL;

public static class BalDependencyInjection
{
    public static IServiceCollection AddBalDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddServices()
            .AddHttpClients(configuration);
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IWarningService, WarningService>();
        return services;
    }
    private static IServiceCollection AddHttpClients(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddHttpClient<IDisasterEventsClient, DisasterEventsClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["DisasterEventsUrl"]);

            })
            .AddPolicyHandler(PolicyStrategies.GetRetryPolicy())
            .AddPolicyHandler(PolicyStrategies.GetCircuitBreakerPolicy());
        return services;
    }
    
}