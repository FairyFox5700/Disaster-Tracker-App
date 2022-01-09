using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
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

    public IObservable<DisasterEvent?> GetDisasterEventByExternalId(string externalId)
    {
        return Observable.FromAsync(async token => await 
            _context.DisasterEvent
                .FirstOrDefaultAsync(e=>e.ExternalApiId==externalId, 
                    cancellationToken: token)
        );
    }

    public IObservable<List<DisasterEvent>> GetDisasterEventsFiltered(
        Expression<Func<DisasterEvent, bool>> predicate)
    {
        return Observable.FromAsync(async token =>   await 
            _context.DisasterEvent
                .Where(predicate)
            .ToListAsync(cancellationToken: token));
    }
    public IObservable<DisasterEvent?> GetLastDisasterEventByClosedTime() 
    {
        return Observable.FromAsync(async token =>await _context.DisasterEvent
            .OrderByDescending(x => x.Closed)
            .FirstOrDefaultAsync(cancellationToken: token));
    }
    public IObservable<Unit> AddEvents(IEnumerable<DisasterEvent> disasterEvents)
    {
        return Observable.FromAsync(async ct =>
        {
            await _context.AddRangeAsync(disasterEvents, ct);
            await _context.SaveChangesAsync(ct);
            return Unit.Default;
        });
    }
    public IObservable<List<Tuple<CalendarEvent, DisasterEvent>>> GetDisasterEventsByCalendarInRadius(Expression<Func<CalendarEvent, bool>>calendarPredicate, int distance) 
    {
        return Observable.FromAsync(async ct =>
            await (from c in _context.CalendarEvents.Where(calendarPredicate)
                from d in _context.DisasterEvent.Where(x => x.Geometry.Distance(c.Coordinates) <= distance)
                select new Tuple<CalendarEvent, DisasterEvent>(c, d)).ToListAsync(ct));
        
    }
}