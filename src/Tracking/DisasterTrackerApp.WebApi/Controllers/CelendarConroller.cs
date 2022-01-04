using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisasterTrackerApp.Identity.Controllers;

public class CelndarConroller:ControllerBase
{
    /// <summary>
    /// Lists the authenticated user's Calendars.
    /// Specifying the <see cref="AuthorizeAttribute"/> will guarantee that the code executes only if the
    /// user is authenticated.
    /// No scopes are required via attributes.
    /// Instead, scope are required via code using <see cref="IGoogleAuthProvider.RequireScopesAsync(string[])"/>.
    /// </summary>
    /// <param name="auth">The Google authorization provider.
    /// This can also be injected on the controller constructor.</param>
    [Authorize]
    public async Task<IActionResult> CalendarList([FromServices] IGoogleAuthProvider auth)
    {
        // Check if the required scopes have been granted. 
        if (await auth.RequireScopesAsync(CalendarService.ScopeConstants.CalendarReadonly) is IActionResult authResult)
        {
            // If the required scopes are not granted, then a non-null IActionResult will be returned,
            // which must be returned from the action. This triggers incremental authorization.
            // Once the user has granted the scope, an automatic redirect to this action will be issued.
            return authResult;
        }
       
        // The required scopes have now been granted.
        GoogleCredential cred = await auth.GetCredentialAsync();
        var service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = cred
        });
        var calendars = await service.CalendarList.List().ExecuteAsync();
        var calendarIds = calendars.Items.Select(calendar => calendar.Id).ToList();
        return Ok(calendarIds);
    }
}