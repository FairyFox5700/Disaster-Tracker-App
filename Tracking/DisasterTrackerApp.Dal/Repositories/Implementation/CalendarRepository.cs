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
    
    public async Task SaveAsync(GoogleCalendar calendar)
    {
        await _context.Calendars.AddAsync(calendar);
        await _context.SaveChangesAsync();
    }

    public async Task<GoogleCalendar?> GetAsync(Guid calendarId)
    {
        return await _context.Calendars.FindAsync(calendarId);
    }

    public async Task<IEnumerable<GoogleCalendar>> GetFilteredAsync(Expression<Func<GoogleCalendar, bool>> predicate)
    {
        return await _context.Calendars
            .Where(predicate)
            .ToListAsync();
    }

    public async Task UpdateAsync(GoogleCalendar oldCalendar, GoogleCalendar newCalendar)
    {
        /*newCalendar.Id = oldCalendar.Id;
        _context.Entry(oldCalendar).CurrentValues.SetValues(newCalendar);*/
        
        oldCalendar.Description = newCalendar.Description;
        oldCalendar.Summary = newCalendar.Summary;
        
        await _context.SaveChangesAsync();
    }
}