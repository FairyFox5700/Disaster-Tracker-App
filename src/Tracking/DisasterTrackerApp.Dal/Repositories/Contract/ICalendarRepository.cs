using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface ICalendarRepository
{
    Task<List<CalendarEvent>> GetCalendarEventsFiltered(Expression<Func<CalendarEvent, bool>> predicate);
    void DeleteCalendarEventsForUser(string userId, List<CalendarEvent> events);
}