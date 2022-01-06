using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.BL.Contract;

public interface IUsersGoogleCalendarDataUpdatingService
{
    Task UpdateUserData(GoogleUser user);
    Task RegisterWatch(Guid userId);
}