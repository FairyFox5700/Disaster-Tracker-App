using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.Helpers;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.Internal;
using Google.Apis.Calendar.v3.Data;
using Microsoft.Extensions.Logging;

namespace DisasterTrackerApp.BL.Implementation;

public class UsersGoogleCalendarDataUpdatingService : IUsersGoogleCalendarDataUpdatingService
{
    private readonly ILogger<UsersGoogleCalendarDataUpdatingService> _logger;
    private readonly ICalendarRepository _calendarRepository;
    private readonly ICalendarEventsRepository _calendarEventsRepository;
    private readonly IGoogleUserRepository _userRepository;
    private readonly ICalendarEventConverter _eventConverter;
    private readonly IGoogleCalendarService _calendarService;
    private readonly ITempRedis _redis;

    public UsersGoogleCalendarDataUpdatingService(
        ILogger<UsersGoogleCalendarDataUpdatingService> logger,
        ICalendarRepository calendarRepository, 
        ICalendarEventsRepository calendarEventsRepository, 
        ICalendarEventConverter eventConverter,
        IGoogleUserRepository userRepository, 
        IGoogleCalendarService calendarService,
        ITempRedis redis)
    {
        _logger = logger;
        _calendarRepository = calendarRepository;
        _calendarEventsRepository = calendarEventsRepository;
        _eventConverter = eventConverter;
        _userRepository = userRepository;
        _calendarService = calendarService;
        _redis = redis;
    }

    public async Task UpdateUserData(GoogleUser user)
    {
        var primaryCalendar = await UpdateUserCalendar(user.UserId);
        if (primaryCalendar == null)
        {
            return;
        }
        
        await _userRepository.SetLastVisitDateTime(user);
        
        var events = await _calendarService.GetUserEventsAsync(user.UserId, primaryCalendar.GoogleCalendarId, user.LastVisit);
        await UpdateCalendarEvents(primaryCalendar.Id, events);
    }

    public async Task<string?> RegisterWatch(Guid userId)
    {
        var calendar = await _calendarRepository.GetFilteredAsync(c => c.UserId == userId);
        var primaryCalendar = calendar.FirstOrDefault(c => c.Primary.HasValue && c.Primary.Value);
        if (primaryCalendar == null)
        {
            _logger.LogWarning("Cannot find primary calendar for user {Id}", userId.ToString());

            return null;
        }
        
        var channel = await _calendarService.WatchEvents(userId, primaryCalendar.GoogleCalendarId);

        return channel.Token;
    }

    public async Task<bool> StopWatch(string channelToken)
    {
        return await _calendarService.StopWatchEvents(channelToken);
    }

    public async Task UpdateCalendarEventsOnDemand(string channelToken, DateTime triggerTime)
    {
        var watchData = _redis.Get<WatchData>(channelToken);
        var calendar = (await _calendarRepository.GetFilteredAsync(c => c.UserId == watchData.UserId))
                        .FirstOrDefault();
        
        if (calendar == null)
        {
            _logger.LogWarning("Cannot find primary calendar for user {Id}" +
                               "Unable to process watched resource notification. Token: {Token}", 
                      watchData.UserId.ToString(), channelToken);
            return;
        }
        
        var events = await _calendarService.GetUserEventsAsync(watchData.UserId, calendar.GoogleCalendarId, watchData.LastTimeTriggered);
        await UpdateCalendarEvents(calendar.Id, events);
        
        _redis.Set(channelToken, watchData.UpdateTriggerTime(triggerTime));
    }

    private async Task<GoogleCalendar?> UpdateUserCalendar(Guid userId)
    {
        var primaryCalendar = await _calendarService.GetUserCalendarAsync(userId);
        if (primaryCalendar == null)
        {
            _logger.LogWarning("Cannot find primary calendar for user {Id}", userId.ToString());
            
            return null;
        }
        
        var existingUserCalendar = (await _calendarRepository.GetFilteredAsync(
            c => c.GoogleCalendarId == primaryCalendar.GoogleCalendarId)).FirstOrDefault();

        if (existingUserCalendar != null)
        {
            await _calendarRepository.UpdateAsync(existingUserCalendar, primaryCalendar);
            return existingUserCalendar;
        }
        
        await _calendarRepository.SaveAsync(primaryCalendar);
        
        return primaryCalendar;
    }

    private async Task UpdateCalendarEvents(Guid calendarId, IEnumerable<Event> calendarEvents)
    {
        var changedCalendarEvents = calendarEvents.ToList();
        var existingCalendarEvents = (await _calendarEventsRepository.GetCalendarEventsFilteredAsync(
                e => e.CalendarId == calendarId))
            .ToList();

        var eventsToDelete = changedCalendarEvents
            .Select(e => e.Id)
            .Intersect(existingCalendarEvents.Select(e => e.GoogleEventId))
            .Select(id => existingCalendarEvents.First(e => e.GoogleEventId == id));
        
        await _calendarEventsRepository.RemoveCalendarEventsAsync(eventsToDelete);

        await RemoveOutdatedCalendarEvents(existingCalendarEvents);

        var updatedEvents = await Task.WhenAll(changedCalendarEvents
            .Where(e => e.Status != "cancelled")
            .Select(async e => await _eventConverter.ToCalendarEventEntity(e, calendarId)));

        await _calendarEventsRepository.SaveEventsAsync(updatedEvents);
    }

    private async Task RemoveOutdatedCalendarEvents(IEnumerable<CalendarEvent> calendarEvents)
    {
        var outdatedEvents = calendarEvents.Where(IsOutdated);
        await _calendarEventsRepository.RemoveCalendarEventsAsync(outdatedEvents);
    }

    private bool IsOutdated(CalendarEvent @event)
    {
        return @event.EndTs < DateTime.UtcNow;
    }
}