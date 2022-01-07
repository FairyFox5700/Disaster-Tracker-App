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
    public async Task<List<CalendarEvent>> GetFilteredAsync(Expression<Func<CalendarEvent, bool>> predicate)
    {
        return await _context.CalendarEvents
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<List<CalendarEvent>> GetFilteredWithUserIdAsync(Guid userId, Expression<Func<CalendarEvent, bool>> predicate)
    {
        return await _context.CalendarEvents
            .Where(predicate)
            .Where(ev => ev.Calendar.UserId == userId)
            .ToListAsync();
    }

    public async Task SaveRangeAsync(IEnumerable<CalendarEvent> events)
    {
        await _context.CalendarEvents.AddRangeAsync(events);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveUserEventsAsync(IEnumerable<CalendarEvent> events)
    {
        _context.CalendarEvents.RemoveRange(events);
        await _context.SaveChangesAsync();
    }
}