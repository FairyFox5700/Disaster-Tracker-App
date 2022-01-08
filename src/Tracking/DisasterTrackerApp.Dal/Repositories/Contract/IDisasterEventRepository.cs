using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface IDisasterEventRepository
{
    Task<DisasterEvent?> GetDisasterEventByExternalId(string externalId);
    Task<List<DisasterEvent>> GetDisasterEventsFiltered(Expression<Func<DisasterEvent,bool>> predicate);
    Task<DisasterEvent?> GetLastDisasterEventByClosedTime();
    Task AddEvents(IEnumerable<DisasterEvent> disasterEvents);
    Task<List<Tuple<CalendarEvent, DisasterEvent>>> GetDisasterEventsByCalendarInRadius(Expression<Func<CalendarEvent, bool>> calendarPredicate, int distance);
}