using System.Linq.Expressions;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class CalendarEventsRepository: ICalendarEventsRepository
{
    private readonly DisasterTrackerContext _context;

    public CalendarEventsRepository(DisasterTrackerContext context)
    {
        _context = context;
    }
    public async Task<List<CalendarEvent>> GetCalendarEventsFilteredAsync(Expression<Func<CalendarEvent, bool>> predicate)
    {
        return await _context.CalendarEvents
            .Where(predicate)
            .ToListAsync();
    }
    
    public async Task SaveEventsAsync(IEnumerable<CalendarEvent> events)
    {
        await _context.CalendarEvents.AddRangeAsync(events);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveCalendarEventsAsync(IEnumerable<CalendarEvent> calendarEvents)
    {
        _context.CalendarEvents.RemoveRange(calendarEvents);
        await _context.SaveChangesAsync();

    }
}