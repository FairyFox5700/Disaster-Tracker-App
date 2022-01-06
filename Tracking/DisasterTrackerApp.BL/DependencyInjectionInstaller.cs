using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.Helpers;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.BL.HttpClients.Implementation;
using DisasterTrackerApp.BL.Implementation;
using DisasterTrackerApp.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DisasterTrackerApp.BL;

public static class DependencyInjectionInstaller
{
    public static IServiceCollection AddBlDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IGoogleOAuthHttpClient, GoogleOAuthHttpClient>();
        
        services.Configure<GoogleOAuthClientCredentials>(
            configuration.GetSection(GoogleOAuthClientCredentials.OAuthCredentials));

        services.Configure<GoogleWebHookOptions>(configuration.GetSection(GoogleWebHookOptions.Section));
        
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IGoogleApiAccessService, GoogleApiAccessService>();
        services.AddScoped<IGoogleCalendarService, GoogleCalendarService>();
        services.AddScoped<ICalendarEventConverter, CalendarEventConverter>();
        services.AddScoped<IUsersGoogleCalendarDataUpdatingService, UsersGoogleCalendarDataUpdatingService>();
        services.AddScoped<IGoogleGeocoderService, GoogleGeocoderService>();
        return services;
    }
}