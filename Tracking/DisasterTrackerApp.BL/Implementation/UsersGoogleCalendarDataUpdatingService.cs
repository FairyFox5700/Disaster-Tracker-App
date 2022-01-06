using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.Helpers;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Google.Apis.Calendar.v3.Data;

namespace DisasterTrackerApp.BL.Implementation;

public class UsersGoogleCalendarDataUpdatingService : IUsersGoogleCalendarDataUpdatingService
{
    private readonly ICalendarRepository _calendarRepository;
    private readonly ICalendarEventsRepository _calendarEventsRepository;
    private readonly ICalendarEventConverter _eventConverter;
    private readonly IGoogleUserRepository _userRepository;
    private readonly IGoogleCalendarService _calendarService;

    public UsersGoogleCalendarDataUpdatingService(
        ICalendarRepository calendarRepository, 
        ICalendarEventsRepository calendarEventsRepository, 
        ICalendarEventConverter eventConverter,
        IGoogleUserRepository userRepository, 
        IGoogleCalendarService calendarService)
    {
        _calendarRepository = calendarRepository;
        _calendarEventsRepository = calendarEventsRepository;
        _eventConverter = eventConverter;
        _userRepository = userRepository;
        _calendarService = calendarService;
    }

    public async Task UpdateUserData(GoogleUser user)
    {
        var calendars = await UpdateUserCalendars(user.UserId);
        foreach (var calendar in calendars)
        {
            var events = await _calendarService.GetUserEventsAsync(user.UserId, calendar.GoogleCalendarId, user.LastDataUpdate);
            await UpdateCalendarEvents(calendar.Id, events);
        }
        
        await _userRepository.SetLastDataUpdateDateTime(user);
    }

    public async Task RegisterWatch(Guid userId)
    {
        var calendars = await _calendarRepository.GetCalendarsFilteredAsync(c => c.UserId == userId);
        await Task.WhenAll(calendars.Select(async c => await _calendarService.WatchEvents(userId, c.Id)));
    }

    private async Task<IEnumerable<GoogleCalendar>> UpdateUserCalendars(Guid userId)
    {
        var actualCalendars = (await _calendarService.GetUserCalendarsAsync(userId)).ToList();
        var existingUserCalendars = (await _calendarRepository.GetCalendarsFilteredAsync(
            c => c.UserId == userId)).ToList();
        
        var calendarsToDelete = existingUserCalendars.Except(actualCalendars, new GoogleCalendarGoogleIdComparer()).ToList();
        await _calendarRepository.RemoveCalendarsAsync(calendarsToDelete);
        
        var calendarsToSave = actualCalendars.Except(existingUserCalendars, new GoogleCalendarGoogleIdComparer()).ToList();
        await _calendarRepository.SaveCalendarsAsync(calendarsToSave);

        return (await _calendarRepository.GetCalendarsFilteredAsync(c => c.UserId == userId)).ToList();
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