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
    
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("register")]
    [ProducesResponseType(typeof(ApiResponse<RegistrationResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Register([FromQuery] string code, [FromQuery] string scope, CancellationToken cancellationToken)
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
        
        var lastVisitTime = await _registrationService.LoginUser(userIdGuid);
        if (lastVisitTime == default)
        {
            var errorMessage = "Something bad happened. " +
                               "Unable to login user because of network failure or user doesn't exist";
            
            return Unauthorized(new ApiError(ErrorCode.LoginError, errorMessage));
        }
        
        var response = new LoginResponse
        {
            LastVisitDateTime = lastVisitTime.Value
        };

        return Ok(new ApiResponse<LoginResponse>(response));
    }
}