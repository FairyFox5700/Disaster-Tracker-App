using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.BL.Contract;

public interface IUsersGoogleCalendarDataUpdatingService
{
    Task UpdateUserData(GoogleUser user);
    Task UpdateCalendarEventsOnWebHook(string channelToken, DateTime triggerTime);
    Task<string?> RegisterWatch(Guid userId);
    Task<bool> StopWatch(string channelToken);
}