using System.Linq.Expressions;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class CalendarRepository : ICalendarRepository
{    
    private readonly DisasterTrackerContext _context;

    public CalendarRepository(DisasterTrackerContext context)
    {
        _context = context;
    }
    
    public async Task SaveCalendarsAsync(IEnumerable<GoogleCalendar> calendars)
    {
        await _context.Calendars.AddRangeAsync(calendars);
        await _context.SaveChangesAsync();
    }

    public async Task<GoogleCalendar?> GetCalendarAsync(Guid calendarId)
    {
        return await _context.Calendars.FindAsync(calendarId);
    }

    public async Task<IEnumerable<GoogleCalendar>> GetCalendarsFilteredAsync(Expression<Func<GoogleCalendar, bool>> predicate)
    {
        return await _context.Calendars
            .Where(predicate)
            .ToListAsync();
    }

    public async Task RemoveCalendarsAsync(IEnumerable<GoogleCalendar> calendars)
    {
        _context.Calendars.RemoveRange(calendars);
        await _context.SaveChangesAsync();

    }
}