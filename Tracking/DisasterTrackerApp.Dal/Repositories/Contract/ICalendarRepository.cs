using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface ICalendarRepository
{
    Task SaveAsync(GoogleCalendar calendar);
    Task UpdateAsync(GoogleCalendar oldCalendar, GoogleCalendar calendar);
    Task<GoogleCalendar?> GetAsync(Guid calendarId);
    Task<IEnumerable<GoogleCalendar>> GetFilteredAsync(Expression<Func<GoogleCalendar, bool>> predicate);
}