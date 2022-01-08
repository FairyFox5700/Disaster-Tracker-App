using System.Net;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.ApiModels.Base;
using DisasterTrackerApp.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace DisasterTrackerApp.WebApi.Controllers;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly GoogleOAuthClientCredentials _credentialOptions;

    public UsersController(
        IRegistrationService registrationService, 
        IOptions<GoogleOAuthClientCredentials> credentialOptions)
    {
        _registrationService = registrationService;
        _credentialOptions = credentialOptions.Value;
    }

    [HttpGet("link")]
    [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
    public IActionResult Link()
    {
        var url = _credentialOptions.AuthUri;

        var queryParams = new Dictionary<string, string>
        {
            { "redirect_uri", "https://localhost:7297/register" },
            { "prompt", "consent" },
            { "response_type", "code" },
            { "client_id", _credentialOptions.ClientId },
            { "scope", "https://www.googleapis.com/auth/calendar.events.readonly https://www.googleapis.com/auth/calendar.readonly openid" },
            { "access_type", "offline" }
        };

        var googleRegistrationUrl = new Uri(QueryHelpers.AddQueryString(url, queryParams!));

        return Ok(googleRegistrationUrl);
    }
    
    [HttpGet("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiError), (int) HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Login([FromQuery] string userId)
    {
        if (Guid.TryParse(userId, out var userIdGuid) == false)
        {
            return BadRequest(new ApiError(ErrorCode.InvalidRequestFormat, "Invalid user id format"));
        }
        
        var watchToken = await _registrationService.LoginUser(userIdGuid);
        if (watchToken == null)
        {
            var errorMessage = "Something bad happened. " +
                               "Unable to login user because of network failure or user doesn't exist";
            
            return Unauthorized(new ApiError(ErrorCode.LoginError, errorMessage));
        }

        var response = new LoginResponse
        {
            WatchToken = watchToken
        };
        
        return Ok(new ApiResponse<LoginResponse>(response));
    }
}