using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace DisasterTrackerApp.BL.Implementation;

public class GoogleCalendarService
{
    private const string CalendarId = "primary";
    private const string WebHookAddress = "www.vnocsymphony.com/google/notifications";

    public GoogleCalendarService()
    {

    }

    /// <summary>
    /// Method to get all existing events for user
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<Events> GetCalendarEventsAsync(string token)
    {
        var googleCredentials = GoogleCredential.FromAccessToken(token);
        var service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = googleCredentials,
        });
        
        EventsResource.ListRequest request = service.Events.List(CalendarId);
        request.TimeMin = DateTime.Now;
        request.ShowDeleted = false;
        request.SingleEvents = true;
        request.MaxResults = 10;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        Events events = await request.ExecuteAsync();
        return events;
    }
    //https://github.com/quynhnhu-cute/BEAUTY-COSMETICS/tree/0017620091deb8918be9a42b8c063ed1ad0ef540
    //https://github.com/anikdey96/CSE_DEPARTMENT/blob/467231204e583b2fc5395ab98bf263c28a856fe0/Controllers/GoogleClassRoomController.cs
    //https://github.com/2-men-team/disaster-tracker/blob/3c0c451b580ac6b524b86f3687bb73c1952c07a4/src/main/java/com/github/twomenteam/disastertracker/service/impl/GoogleApiServiceImpl.java
    
    //TODO pass calendar id 
    //TODO check format of date
    public async Task<Events> GetCalendarEventsAsyncByDate(string token, DateTime dateTime)
    {
        var googleCredentials = GoogleCredential.FromAccessToken(token);
        var service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = googleCredentials,
        });
        service.CalendarList.Watch(BuildWatchChannel(token));
        EventsResource.ListRequest request = service.Events.List(CalendarId);
        request.UpdatedMin = dateTime;
        request.ShowDeleted = false;
        request.SingleEvents = true;
        request.MaxResults = 10;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        Events events = await request.ExecuteAsync();
        return events;
    }

    public async Task<Channel> WatchEvents(string token, string apiKey)
    {
        var googleCredentials = GoogleCredential.FromAccessToken(token);
        var service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = googleCredentials,
        });
        return await service.Events.Watch(BuildWatchChannel( apiKey), CalendarId).ExecuteAsync();
    }
    private Channel BuildWatchChannel( string apiKey)
    {
        Channel c = new Channel
        {
            Address = WebHookAddress,
            Id = Guid.NewGuid().ToString(),
            Type = "web_hook",
            Token = apiKey, // space Id could go here
            Expiration = long.MaxValue,
            //ResourceUri = @"https://www.googleapis.com/calendar/v3/calendars/" + calendarid+ "/events"
        };
        return c;
    }
}
/*
public Mono<Void> updateCalendarEvents(FetchedCalendarEvent event) {
    var newCalendarEvent = event.getCalendarEvent();

    if (event.getStatus() == FetchedCalendarEvent.Status.DELETE) {
        return calendarEventRepository.deleteAllByGoogleIdAndUserId(
            newCalendarEvent.getGoogleId(), newCalendarEvent.getUserId());
    }

    return calendarEventRepository
        .findByGoogleIdAndUserId(newCalendarEvent.getGoogleId(), newCalendarEvent.getUserId())
        .flatMap(oldEvent -> warningRepository
            .deleteAllByCalendarEventId(oldEvent.getId())
            .thenReturn(oldEvent))
        .map(oldEvent -> oldEvent.toBuilder()
            .start(newCalendarEvent.getStart())
            .end(newCalendarEvent.getEnd())
            .summary(newCalendarEvent.getSummary())
            .location(newCalendarEvent.getLocation())
            .build()
            .withCoordinates(newCalendarEvent.getCoordinates()))
        .switchIfEmpty(Mono.just(newCalendarEvent))
        .flatMap(calendarEventRepository::save)
        .then();
}*/