namespace DisasterTrackerApp.BL.Contract;

public interface IRegistrationService
{
    Task<Guid> RegisterUser(string authorizationCode, CancellationToken cancellationToken);
    Task<DateTime?> LoginUser(Guid userId);
}