using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.BL.Helpers;

public class GoogleCalendarGoogleIdComparer: IEqualityComparer<GoogleCalendar>

{
    public bool Equals(GoogleCalendar? x, GoogleCalendar? y)
    {
        return x?.GoogleCalendarId == y?.GoogleCalendarId;
    }

    public int GetHashCode(GoogleCalendar obj)
    {
        return obj.GoogleCalendarId.GetHashCode();
    }
}