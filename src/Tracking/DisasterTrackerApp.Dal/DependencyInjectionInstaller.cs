using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Dal.Repositories.Implementation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DisasterTrackerApp.Dal;

public static class DependencyInjectionInstaller
{
    public static IServiceCollection AddDalDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddRedisDependencies(configuration)
            .AddDbConnections(configuration)
            .RegisterHangfire(configuration)
            .AddRepositories();
        return services;
    }

    private static IServiceCollection AddRedisDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(opt => 
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisCacheConnection")));

        return services;
    }
    private static IServiceCollection AddDbConnections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DisasterTrackerContext>(
            options =>options.UseNpgsql(configuration.GetConnectionString("DisasterTrackerConnection"),
                o => o.UseNetTopologySuite()
                    .MigrationsAssembly(typeof(DisasterTrackerContext).Assembly.FullName)));
        
        return services;
    }

    private static IServiceCollection RegisterHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(x => x.UsePostgreSqlStorage(configuration
            .GetConnectionString("DisasterTrackerConnection")));
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IDisasterEventRepository, DisasterEventRepository>();
        services.AddTransient<ICalendarRepository, CalendarRepository>();
        services.AddTransient<IRedisDisasterEventsRepository, RedisDisasterEventsRepository>();
        return services;
    }
}