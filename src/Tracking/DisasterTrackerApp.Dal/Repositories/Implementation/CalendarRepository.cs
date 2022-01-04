using System.Linq.Expressions;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class CalendarRepository: ICalendarRepository
{
    private readonly DisasterTrackerContext _context;

    public CalendarRepository(DisasterTrackerContext context)
    {
        _context = context;
    }
    public Task<List<CalendarEvent>> GetCalendarEventsFiltered(Expression<Func<CalendarEvent, bool>> predicate)
    {
        return _context.CalendarEvents
            .Where(predicate)
            .ToListAsync();
    }

    public void DeleteCalendarEventsForUser(string userId, List<CalendarEvent> events)
    {
        _context.CalendarEvents.RemoveRange(events);
    }
}