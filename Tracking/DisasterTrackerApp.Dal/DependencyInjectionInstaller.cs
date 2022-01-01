using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Dal.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DisasterTrackerApp.Dal;

public static class DependencyInjectionInstaller
{
    public static IServiceCollection AddDalDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DisasterTrackerContext>(
            options =>options.UseNpgsql(configuration.GetConnectionString("DisasterTrackerConnection"),
                o => o.UseNetTopologySuite()
                    .MigrationsAssembly(typeof(DisasterTrackerContext).Assembly.FullName)));

        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IDisasterEventRepository, DisasterEventRepository>();
        services.AddTransient<ICalendarRepository, CalendarRepository>();
        return services;
    }
}