using System.Net;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.ApiModels.Base;
using DisasterTrackerApp.Models.Calendar;
using Microsoft.AspNetCore.Mvc;

namespace DisasterTrackerApp.WebApi.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class GoogleRequestsReceivingController : ControllerBase
{
    private readonly IUsersGoogleCalendarDataUpdatingService _calendarUpdateService;
    private readonly IRegistrationService _registrationService;

    public GoogleRequestsReceivingController(
        IUsersGoogleCalendarDataUpdatingService calendarUpdateService, 
        IRegistrationService registrationService)
    {
        _calendarUpdateService = calendarUpdateService;
        _registrationService = registrationService;
    }
    
    [HttpPost("receive-updates")]
    public async Task<IActionResult> ReceiveCalendarEventUpdate(
        [FromHeader(Name = "X-Goog-Channel-Token")] string token,
        [FromHeader(Name = "X-Goog-Resource-State")] string resourceState)
    {
        if (resourceState.Equals(ResourceState.Sync))
        {
            return Ok();
        }
        
        await _calendarUpdateService.UpdateCalendarEventsOnWebHook(token, DateTime.UtcNow);
        return Ok();
    }
    
    [HttpGet("register")]
    [ProducesResponseType(typeof(ApiResponse<RegistrationResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Register([FromQuery] string code, CancellationToken cancellationToken)
    {
        var userId = await _registrationService.RegisterUser(code, cancellationToken);

        if (userId == default)
        {
            var errorMessage = "Something bad happened. " +
                               "Unable to register user because of network failure or user is already registered";
            
            return BadRequest(new ApiError(ErrorCode.InternalError, errorMessage));
        }
        
        var response = new RegistrationResponse
        {
            UserId = userId.ToString()
        };

        return Ok(new ApiResponse<RegistrationResponse>(response));
    }
}