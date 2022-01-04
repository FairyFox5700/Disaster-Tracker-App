using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.Disaster;
using DisasterTrackerApp.WebApi.HttpClients.Contract;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace DisasterTrackerApp.Identity.Controllers;

public class Cd : ControllerBase
{
    private readonly IDisasterEventsClient _disasterEventsClient;
    private readonly IRedisDisasterEventsRepository _redisDisasterEventsRepository;
    
    private readonly ICalendarRepository _calendarRepository;

    public Cd(IDisasterEventsClient disasterEventsClient,
        IRedisDisasterEventsRepository redisDisasterEventsRepository,
        ICalendarRepository calendarRepository)
    {
        _disasterEventsClient = disasterEventsClient;
        _redisDisasterEventsRepository = redisDisasterEventsRepository;
        _calendarRepository = calendarRepository;
    }

    private const int MAX_RADIUS = 40;
    public IDisasterEventsClient DisasterEventsClient => _disasterEventsClient;

    public record WarningDto(Guid CalendarId,
        Guid DisasterId,
        string Description,
        DateTimeOffset? EndTs,
        DateTimeOffset? StartTs)
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }

    public record GetWarningRequest(
        string UserId,
        DateTimeOffset? EndDateTimeOffset,
        DateTimeOffset? StartDateTimeOffset);

    private Expression<Func<CalendarEvent, bool>> BuildExpression(GetWarningRequest warningRequest)
    {
        if(warningRequest.StartDateTimeOffset != null && warningRequest.EndDateTimeOffset != null)
        {
            return date => date.StartedTs <= warningRequest.StartDateTimeOffset && date.EndTs >= warningRequest.EndDateTimeOffset;
        }

        if (warningRequest.StartDateTimeOffset != null)
        {
            return date => date.StartedTs <= warningRequest.StartDateTimeOffset;
        }

        if (warningRequest.EndDateTimeOffset != null)
        {
            return date => date.EndTs >= warningRequest.EndDateTimeOffset;
        }

        return data => true;
    }

    [HttpGet("/receive-warnings")]
    public async Task GetWarnings(GetWarningRequest warningRequest, CancellationToken cancellationToken=default)
    {
        
        var response = Response;
        response.Headers.Add("Content-Type", "text/event-stream");
        var data = Observable.FromAsync(async () =>
            from c in await _calendarRepository.GetCalendarEventsFilteredWithUserId(warningRequest.UserId,
                BuildExpression(warningRequest),
                cancellationToken)
            from d in _redisDisasterEventsRepository.GetAllDisasterEvents()
            where c.Coordiantes.Distance(d.Coordiantes) < MAX_RADIUS
            select new WarningDto(c.Id,
                d.Id,
                $"Warning. Disaster can occur near your event in place {c.Location}",
                c.EndTs,
                c.StartedTs
            ))
            .SelectMany(e=>e);
        await _disasterEventsClient.GetDisasterEventsAsync(cancellationToken)
            .Merge<object>( data)
            .SelectMany(async e =>
            {
                await response.WriteAsync($"{JsonConvert.SerializeObject(e)}\r\r", cancellationToken: cancellationToken);
                await response.Body.FlushAsync(cancellationToken);
                return e;
            })
            .ToTask(cancellationToken);
    }
}