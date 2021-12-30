using DisasterTracker.Identity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DisasterTracker.Identity.DAL;

public static class IdentityDependencyInjection
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<IdentityContext>(cfg => 
                cfg.UseSqlServer(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                  .AddEntityFrameworkStores<IdentityContext>()
                  .AddDefaultTokenProviders();

            return services;
        }
}