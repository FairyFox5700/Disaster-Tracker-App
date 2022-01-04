using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Microsoft.EntityFrameworkCore;
using Point = NetTopologySuite.Geometries.Point;

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class CalendarRepository: ICalendarRepository
{
    private readonly DisasterTrackerContext _context;

    public CalendarRepository(DisasterTrackerContext context)
    {
        _context = context;
    }
    public async Task<List<CalendarEvent>> GetAllCalendars(CancellationToken cancellationToken)
    {
        return await _context.CalendarEvents
                .ToListAsync(cancellationToken);
    }
    public Task<List<CalendarEvent>> GetCalendarEventsFilteredWithUserId(string userId,
        Expression<Func<CalendarEvent, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return
           Task.FromResult(
               new List<CalendarEvent>()
            {
                new CalendarEvent()
                {
                    Id = Guid.NewGuid(),
                    GoogleEventId = "1212",
                    Calendar = null,
                    CalendarId = "sas",
                    Location = "Kiev",
                    Summary = "first event",
                    Coordiantes = new Point(12, 2),
                    StartedTs = DateTime.UtcNow,
                    EndTs = DateTime.UtcNow,
                },
                new CalendarEvent()
                {
                    Id = Guid.NewGuid(),
                    GoogleEventId = "1212",
                    Calendar = null,
                    CalendarId = "sas",
                    Location = "Kiev",
                    Summary = "first event",
                    Coordiantes = new Point(12, 2),
                    StartedTs = DateTime.UtcNow,
                    EndTs = DateTime.UtcNow,
                }
                ,  new CalendarEvent()
                {
                    Id = Guid.NewGuid(),
                    GoogleEventId = "1212",
                    Calendar = null,
                    CalendarId = "sas",
                    Location = "Kiev",
                    Summary = "first event",
                    Coordiantes = new Point(2.822, 42.71299),
                    StartedTs = DateTime.UtcNow,
                    EndTs = DateTime.UtcNow,
                },
                new CalendarEvent()
                {
                    Id = Guid.NewGuid(),
                    GoogleEventId = "1212",
                    Calendar = null,
                    CalendarId = "sas",
                    Location = "Kiev",
                    Summary = "first event",
                    Coordiantes = new Point(-78.341, -2.005),
                    StartedTs = DateTime.UtcNow,
                    EndTs = DateTime.UtcNow,
                }
            });/*
               (from c in  _context.Calendars
                where Equals(c.UserId, userId)
                join ce in _context.CalendarEvents on c.GoogleCalendarId equals ce.CalendarId
                select ce)
               .Where(predicate)
               .ToListAsync(cancellationToken);*/
    }

    public Task<List<CalendarEvent>> GetCalendarEventsFiltered(Expression<Func<CalendarEvent, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return _context.CalendarEvents
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }
    public void DeleteCalendarEventsForUser(string userId, List<CalendarEvent> events,CancellationToken cancellationToken)
    {
        _context.CalendarEvents.RemoveRange(events);
    }
}