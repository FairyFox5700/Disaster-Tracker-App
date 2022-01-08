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

    public WarningController(IWarningService warningService, IGoogleCalendarService calendarService)
    {
        _warningService = warningService;
        _calendarService = calendarService;
    }
    
    [HttpGet("/receive-warnings")]
    public async Task GetWarnings(WarningRequest warningRequest, CancellationToken cancellationToken=default)
    {
        var response = Response;
        response.Headers.Add("Content-Type", "text/event-stream");
        await _warningService.GetWarningEvents(warningRequest, cancellationToken)
            .DefaultIfEmpty(new WarningDto(default,default,default,default,default))
            .SelectMany(async e =>
            {
                await response.WriteAsync($"{JsonConvert.SerializeObject(e)}\r\r",
                    cancellationToken: cancellationToken);
                await response.Body.FlushAsync(cancellationToken);
                return e;
            })
            .ToTask(cancellationToken);

        await _calendarService.StopWatchEvents(warningRequest.UserId); // todo you can temporary remove it for testing purposes
        // after this call our application stops receiving real time updates from user's Google Calendar
    }
}