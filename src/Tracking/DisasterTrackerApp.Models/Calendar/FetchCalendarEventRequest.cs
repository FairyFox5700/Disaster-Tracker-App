using DisasterTrackerApp.Models.Auth;

namespace DisasterTrackerApp.Models.Calendar;

public class FetchCalendarEventRequest
{
    public AuthenticationToken AuthenticationToken { get; set; }
    public List<string> Scopes { get; set; }
    public int UserId { get; set; }
}