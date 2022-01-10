using System.Linq.Expressions;
using System.Reactive;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface IDisasterEventRepository
{
    IObservable<DisasterEvent?> GetDisasterEventByExternalId(string externalId);
    IObservable<List<DisasterEvent>> GetDisasterEventsFiltered(Expression<Func<DisasterEvent, bool>> predicate);
    Task<DisasterEvent?> GetLastDisasterEventByClosedTime();
    Task AddEvents(IEnumerable<DisasterEvent> disasterEvents);
    IObservable<List<Tuple<CalendarEvent, DisasterEvent>>> GetDisasterEventsByCalendarInRadius(Expression<Func<CalendarEvent, bool>>
        calendarPredicate, int distance);
}