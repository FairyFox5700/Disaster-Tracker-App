using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface ICalendarRepository
{
    Task<List<CalendarEvent>> GetCalendarEventsFiltered(Expression<Func<CalendarEvent, bool>> predicate,CancellationToken cancellationToken);
    void DeleteCalendarEventsForUser(string userId, List<CalendarEvent> events, CancellationToken cancellationToken);

    Task<List<CalendarEvent>> GetCalendarEventsFilteredWithUserId(string userId,
        Expression<Func<CalendarEvent, bool>> predicate,
        CancellationToken cancellationToken);
    Task<List<CalendarEvent>> GetAllCalendars(CancellationToken cancellationToken);
}