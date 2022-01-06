using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.Configuration;
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

    public GoogleCalendarService(
        IGoogleApiAccessService apiAccessService,
        IGoogleUserRepository usersRepository,
        IOptions<GoogleWebHookOptions> webHookConfiguration)
    {
        _apiAccessService = apiAccessService;
        _usersRepository = usersRepository;
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

    public async Task<IEnumerable<GoogleCalendar>> GetUserCalendarsAsync(Guid userId)
    {
        var service = await InitializeCalendarService(userId);
        var request = service.CalendarList.List();
        //var request = service.Calendars.Get("primary"); // todo
        var calendarList = await request.ExecuteAsync();

        if (calendarList?.Items == null || calendarList.Items.Any() == false)
        {
            return new List<GoogleCalendar>();
        }

        var calendars = calendarList.Items.Select(c => new GoogleCalendar
        {
            Description = c.Description,
            GoogleCalendarId = c.Id,
            Primary = c.Primary,
            Summary = c.Summary,
            UserId = userId
        }).ToList();

        return calendars;
    }

    public async Task<IEnumerable<Event>> GetUserEventsAsync(Guid userId, string googleCalendarId,
        DateTime? updatedAfter = null)
    {
        var service = await InitializeCalendarService(userId);
        var request = service.Events.List(googleCalendarId);
        // var request = service.Events.List("primary"); //todo
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        request.TimeMin = DateTime.Now;
        request.SingleEvents = true;
        request.ShowDeleted = true;
        request.UpdatedMin = updatedAfter;
        var calendarEvents = await request.ExecuteAsync();

        if (calendarEvents?.Items == null || calendarEvents.Items.Any() == false)
        {
            return new List<Event>();
        }

        return calendarEvents.Items;
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

    public async Task<Channel> WatchEvents(Guid userId, Guid calendarId)
    {
        var service = await InitializeCalendarService(userId);

        return await service.Events.Watch(BuildWatchChannel(userId, calendarId), calendarId.ToString()).ExecuteAsync();
    }

    private Channel BuildWatchChannel(Guid userId, Guid calendarId)
    {
        return new Channel
        {
            Address = _webHookConfiguration.WebHookUrl,
            Id = Guid.NewGuid().ToString(),
            Type = "web_hook",
            Token = userId.ToString(),
            Expiration = long.MaxValue,
            ResourceUri = @$"https://www.googleapis.com/calendar/v3/calendars/{calendarId}/events/watch"
        };
    }
}