using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.BL.Contract;

public interface IGoogleApiAccessService
{
    Task<GoogleUser?> GetUserAccessCredentialsAsync(string authorizationCode, CancellationToken cancellationToken);
    Task<bool> TryRefreshAccessTokenForUserAsync(Guid userId, bool force = false);
}