using System.Net;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.Models.Calendar;
using Microsoft.AspNetCore.Mvc;

namespace DisasterTrackerApp.WebApi.Controllers;

[ApiController]
[Route("calendar/updates")]
public class GoogleWebHookController : ControllerBase
{
    private readonly IUsersGoogleCalendarDataUpdatingService _calendarUpdateService;
    
    public GoogleWebHookController(IUsersGoogleCalendarDataUpdatingService calendarUpdateService)
    {
        _calendarUpdateService = calendarUpdateService;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("receive")]
    public async Task<IActionResult> ReceiveEventUpdate(
        [FromHeader(Name = "X-Goog-Channel-Token")] string token,
        [FromHeader(Name = "X-Goog-Resource-State")] string resourceState)
    {
        if (resourceState.Equals(ResourceState.Sync))
        {
            return Ok();
        }
        
        await _calendarUpdateService.UpdateCalendarEventsOnDemand(token, DateTime.UtcNow);
        return Ok();
    }

    [HttpGet("stop")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]

    public async Task<IActionResult> StopReceivingUpdates(string watchToken)
    {
        var success = await _calendarUpdateService.StopWatch(watchToken);

        return success ? Ok() : NotFound();
    }
}