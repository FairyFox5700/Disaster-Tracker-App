using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface ICalendarRepository
{
    Task SaveCalendarsAsync(IEnumerable<GoogleCalendar> calendars);
    Task<GoogleCalendar?> GetCalendarAsync(Guid calendarId);
    Task<IEnumerable<GoogleCalendar>> GetCalendarsFilteredAsync(Expression<Func<GoogleCalendar, bool>> predicate);
    Task RemoveCalendarsAsync(IEnumerable<GoogleCalendar> calendars);
}