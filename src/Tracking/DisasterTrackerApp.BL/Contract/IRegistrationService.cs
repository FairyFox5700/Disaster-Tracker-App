namespace DisasterTrackerApp.BL.Contract;

public interface IRegistrationService
{
    Task<Guid> RegisterUser(string authorizationCode, CancellationToken cancellationToken);
    Task<string?> LoginUser(Guid userId);
}