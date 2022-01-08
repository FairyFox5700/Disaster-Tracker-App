using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface ICalendarEventsRepository
{
    Task<List<CalendarEvent>> GetFilteredAsync(Expression<Func<CalendarEvent, bool>> predicate);
    Task<List<CalendarEvent>> GetFilteredWithUserIdAsync(Guid userId,
        Expression<Func<CalendarEvent, bool>> predicate);
    Task SaveRangeAsync(IEnumerable<CalendarEvent> events);
    Task RemoveUserEventsAsync(IEnumerable<CalendarEvent> events);
}