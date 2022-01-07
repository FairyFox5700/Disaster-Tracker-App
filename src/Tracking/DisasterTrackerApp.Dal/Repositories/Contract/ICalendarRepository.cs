using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface ICalendarRepository
{
    Task SaveAsync(GoogleCalendar calendar);
    Task UpdateAsync(GoogleCalendar oldCalendar, GoogleCalendar newCalendar);
    Task<GoogleCalendar?> GetAsync(Guid calendarId);
    Task<List<GoogleCalendar>> GetFilteredAsync(Expression<Func<GoogleCalendar, bool>> predicate);
}