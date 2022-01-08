using DisasterTrackerApp.Entities;
using Google.Apis.Calendar.v3.Data;

namespace DisasterTrackerApp.BL.Mappers.Contract;

public interface ICalendarEventMapper
{
    Task<CalendarEvent> ToCalendarEventEntity(Event googleEvent, Guid calendarId);
}