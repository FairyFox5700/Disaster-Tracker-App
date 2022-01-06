using DisasterTrackerApp.Entities;
using Google.Apis.Calendar.v3.Data;

namespace DisasterTrackerApp.BL.Helpers;

public interface ICalendarEventConverter
{
    Task<CalendarEvent> ToCalendarEventEntity(Event googleEvent, Guid calendarId);
}