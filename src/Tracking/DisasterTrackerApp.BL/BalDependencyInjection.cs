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
    private const string UserAgentHeaderValue = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 Edg/96.0.1054.62";
    private const string UserAgentHeaderName = "User-Agent";
    private const string ApplicationJsonHeaderValue = "application/json";
    private const string AcceptHeaderName = "Accept";

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
        services.AddTransient<INewClosedEventsService, NewClosedEventsService>();
        return services;
    }
    private static IServiceCollection AddHttpClients(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddHttpClient<IDisasterEventsClient, DisasterEventsClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["DisasterEventsUrl"]);
                client.DefaultRequestHeaders.Add(AcceptHeaderName,ApplicationJsonHeaderValue);
                client.DefaultRequestHeaders
                    .Add(UserAgentHeaderName,UserAgentHeaderValue);
            })
            .AddPolicyHandler(PolicyStrategies.GetRetryPolicy())
            .AddPolicyHandler(PolicyStrategies.GetCircuitBreakerPolicy());
        
        services.AddHttpClient<IClosedDisasterEventsClient, ClosedDisasterEventsClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ClosedDisasterEventUri"]);
            client.DefaultRequestHeaders.Add(AcceptHeaderName, ApplicationJsonHeaderValue);
            client.DefaultRequestHeaders.Add(UserAgentHeaderName, UserAgentHeaderValue);
        })
            .AddPolicyHandler(PolicyStrategies.GetRetryPolicy())
            .AddPolicyHandler(PolicyStrategies.GetCircuitBreakerPolicy());
        
        return services;
    }
    
}