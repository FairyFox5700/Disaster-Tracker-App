using System.Net;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.ApiModels.Base;
using Microsoft.AspNetCore.Mvc;

namespace DisasterTrackerApp.WebApi.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class OAuthController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public OAuthController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }
    
    [HttpGet("register")]
    [ProducesResponseType(typeof(ApiResponse<RegistrationResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Register([FromQuery] string code, 
        [FromQuery] string scope, 
        CancellationToken cancellationToken)
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