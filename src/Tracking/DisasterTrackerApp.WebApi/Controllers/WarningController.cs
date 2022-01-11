using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.Models.Warnings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DisasterTrackerApp.WebApi.Controllers;

public class WarningController : ControllerBase
{
    private readonly IWarningService _warningService;
    private readonly IGoogleCalendarService _calendarService;

    private readonly ConcurrentDictionary<string, WarningDto> _concurrentDictionary =new ();
    public WarningController(IWarningService warningService, IGoogleCalendarService calendarService)
    {
        _warningService = warningService;
        _calendarService = calendarService;
    }
    
    [HttpGet("/receive-warnings")]
    public async Task GetWarnings(WarningRequest warningRequest, CancellationToken cancellationToken=default)
    {
        var response = Response;
        response.ContentType ="text/event-stream; charset=utf-8;";
        response.Headers.Connection = "keep-alive";
        response.Headers.CacheControl = "no-cache";
        
        await Observable.Interval(TimeSpan.FromSeconds(10))
            .SelectMany(_ => _warningService.GetWarningEvents(warningRequest, cancellationToken))
            .SelectMany(async e =>
            {
                if (_concurrentDictionary.ContainsKey($"{e.DisasterId}{e.CalendarId}")) return e;
                await response.WriteAsync($"{JsonConvert.SerializeObject(e)}\r\r",
                    cancellationToken: cancellationToken);
                await response.Body.FlushAsync(cancellationToken);
                return e;
            })
            .Do(p => _concurrentDictionary.AddOrUpdate($"{p.DisasterId}{p.CalendarId}", p, (s, c) => c))
            .Do(e =>Console.WriteLine($"------CURRENT COUNT IN DICT:{_concurrentDictionary.Count}"))
            .Replay(1)
            .RefCount();
        
        await _calendarService.StopWatchEvents(warningRequest.UserId);
    }
    [HttpGet("/receive-statisticwarnings")]
    public async Task GetStatisticsWarnings(WarningRequest warningRequest, CancellationToken cancellationToken = default)
    {
        var response = Response;
        response.ContentType ="text/event-stream; charset=utf-8;";
        
        await _warningService.GetStatisticsWarningEvents(warningRequest, cancellationToken)
            .DefaultIfEmpty(new WarningDto(default, default, default, default, default))
            .SelectMany(async e =>
            {
                await response.WriteAsync($"{JsonConvert.SerializeObject(e)}\r\r",
                    cancellationToken: cancellationToken);
                await response.Body.FlushAsync(cancellationToken);
                return e;
            })
            .ToTask(cancellationToken);

    }
}