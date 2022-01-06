using DisasterTrackerApp.Entities;
using Google.Apis.Calendar.v3.Data;

namespace DisasterTrackerApp.BL.Contract;

public interface IGoogleCalendarService
{
    Task<IEnumerable<GoogleCalendar>> GetUserCalendarsAsync(Guid userId);
    Task<IEnumerable<Event>> GetUserEventsAsync(Guid userId, string calendarId, DateTime? updatedAfter = null);
    Task<Channel> WatchEvents(Guid userId, Guid calendarId);
}