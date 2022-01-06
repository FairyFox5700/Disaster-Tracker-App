using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface ICalendarEventsRepository
{
    Task<List<CalendarEvent>> GetCalendarEventsFilteredAsync(Expression<Func<CalendarEvent, bool>> predicate);
    Task SaveEventsAsync(IEnumerable<CalendarEvent> events);
    Task RemoveCalendarEventsAsync(IEnumerable<CalendarEvent> events);
}