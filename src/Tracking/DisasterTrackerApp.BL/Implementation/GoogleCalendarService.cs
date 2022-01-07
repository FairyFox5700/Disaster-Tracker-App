using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.Calendar;
using DisasterTrackerApp.Models.Configuration;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Options;

namespace DisasterTrackerApp.BL.Implementation;

public class GoogleCalendarService : IGoogleCalendarService
{
    private readonly IGoogleApiAccessService _apiAccessService;
    private readonly IGoogleUserRepository _usersRepository;
    private readonly GoogleWebHookOptions _webHookConfiguration;
    private readonly IRedisWatchChannelsRepository _redis;
    
    public GoogleCalendarService(
        IGoogleApiAccessService apiAccessService,
        IGoogleUserRepository usersRepository,
        IOptions<GoogleWebHookOptions> webHookConfiguration, 
        IRedisWatchChannelsRepository redis)
    {
        _apiAccessService = apiAccessService;
        _usersRepository = usersRepository;
        _redis = redis;
        _webHookConfiguration = webHookConfiguration.Value;
    }

    private async Task<CalendarService> InitializeCalendarService(Guid userId)
    {
        var accessToken = await GetUserAccessTokenAsync(userId);
        var googleCredentials = GoogleCredential.FromAccessToken(accessToken);
        var service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = googleCredentials
        });

        return service;
    }

    public async Task<GoogleCalendar?> GetUserCalendarAsync(Guid userId)
    {
        var service = await InitializeCalendarService(userId);
        var request = service.Calendars.Get("primary");
        var primaryCalendar = await request.ExecuteAsync();

        if (primaryCalendar == null)
        {
            return null;
        }

        var calendar = new GoogleCalendar
        {
            Description = primaryCalendar.Description,
            GoogleCalendarId = primaryCalendar.Id,
            Primary = true,
            Summary = primaryCalendar.Summary,
            UserId = userId
        };

        return calendar;
    }

    public async Task<IEnumerable<Event>> GetUserEventsAsync(Guid userId, string googleCalendarId, DateTime? updatedAfter = null)
    {
        var service = await InitializeCalendarService(userId);
        var request = service.Events.List(googleCalendarId);
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        request.TimeMin = DateTime.UtcNow;
        request.SingleEvents = true;
        request.ShowDeleted = true;
        request.UpdatedMin = updatedAfter;
        var calendarEvents = await request.ExecuteAsync();

        return calendarEvents?.Items ?? new List<Event>();
    }

    private async Task<string> GetUserAccessTokenAsync(Guid userId)
    {
        var user = await _usersRepository.FindUserAsync(userId);
        if (user is null)
        {
            return string.Empty;
        }

        await _apiAccessService.TryRefreshAccessTokenForUserAsync(userId);

        return user.AccessToken;
    }

    public async Task<Channel> WatchEvents(Guid userId, string googleCalendarId)
    {
        var service = await InitializeCalendarService(userId);
        var token = Guid.NewGuid().ToString();

        var channel =  await service.Events.Watch(BuildWatchChannel(token, googleCalendarId), googleCalendarId).ExecuteAsync();

        var watchData = new WatchChannelData(userId, channel.Id, channel.ResourceId);
        _redis.Set(token, watchData);

        return channel;
    }

    public async Task<bool> StopWatchEvents(string channelToken)
    {
        var watchData = _redis.Get<WatchChannelData>(channelToken);
        var service = await InitializeCalendarService(watchData.UserId);

        var channel = new Channel
        {
            Id = watchData.ChannelId,
            ResourceId = watchData.ResourceId
        };
        try
        {
            await service.Channels.Stop(channel).ExecuteAsync();
            return true;
        }
        catch (GoogleApiException)
        {
            return false;
        }
    }

    private Channel BuildWatchChannel(string token, string googleCalendarId)
    {
        return new Channel
        {
            Address = $"{_webHookConfiguration.Address}{_webHookConfiguration.Url}",
            Id = Guid.NewGuid().ToString(),
            Type = "web_hook",
            Token = token,
            Expiration = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeMilliseconds(), // todo adjust expiration time
            ResourceUri = @$"https://www.googleapis.com/calendar/v3/calendars/{googleCalendarId}/events/watch"
        };
    }
}