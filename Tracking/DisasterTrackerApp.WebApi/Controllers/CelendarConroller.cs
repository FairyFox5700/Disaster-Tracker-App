using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.Models.Calendar;
using Microsoft.AspNetCore.Mvc;

namespace DisasterTrackerApp.WebApi.Controllers;

public class CalendarController : ControllerBase
{
    private readonly IUsersGoogleCalendarDataUpdatingService _calendarUpdateService;
    public CalendarController(IUsersGoogleCalendarDataUpdatingService calendarUpdateService)
    {
        _calendarUpdateService = calendarUpdateService;
    }

    [HttpPost("updates")]
    public async Task<IActionResult> ReceiveEventUpdate(
        [FromHeader(Name = "X-Goog-Channel-ID")] string channelId,
        [FromHeader(Name = "X-Goog-Resource-ID")] string eventGoogleId,
        [FromHeader(Name = "X-Goog-Resource-State")] string resourceSate)
    {
        if (resourceSate.Equals(ResourceState.Sync))
        {
            return Ok();
        }
        
        return Ok();
    }
    
    [HttpGet("events")]
    public async Task<IActionResult> Test()
    {
        return Ok();

    }
}