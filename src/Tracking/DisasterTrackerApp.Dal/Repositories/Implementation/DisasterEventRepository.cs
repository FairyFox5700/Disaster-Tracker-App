using System.Linq.Expressions;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class DisasterEventRepository : IDisasterEventRepository
{
    private readonly DisasterTrackerContext _context;

    public DisasterEventRepository(DisasterTrackerContext context)
    {
        _context = context;
    }

    public async Task<DisasterEvent?> GetDisasterEventByExternalId(string externalId)
    {
        return await _context.DisasterEvent.FirstOrDefaultAsync(e=>e.ExternalApiId==externalId);
    }

    public async Task<List<DisasterEvent>> GetDisasterEventsFiltered(Expression<Func<DisasterEvent,bool>> predicate)
    {
        return await _context.DisasterEvent.Where(predicate).ToListAsync();
    }
    public async Task<DisasterEvent?> GetLastDisasterEventByClosedTime() 
    {
        return await _context.DisasterEvent.OrderByDescending(x => x.Closed).FirstOrDefaultAsync().ConfigureAwait(false);
    }
    public async Task AddEvents(IEnumerable<DisasterEvent> disasterEvents)
    {
        await _context.AddRangeAsync(disasterEvents).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
    public async Task<List<Tuple<CalendarEvent,DisasterEvent>>> GetDisasterEventsByCalendarInRadius(Expression<Func<CalendarEvent, bool>>calendarPredicate, int distance) 
    {
        return await (from c in _context.CalendarEvents.Where(calendarPredicate)
                from d in _context.DisasterEvent.Where(x => x.Geometry.Distance(c.Coordinates) <= distance)
                select new Tuple<CalendarEvent, DisasterEvent>(c, d)).ToListAsync();
        
    }
}