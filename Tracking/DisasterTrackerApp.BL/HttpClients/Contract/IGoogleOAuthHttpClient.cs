using DisasterTrackerApp.Models.GoogleOAuth;

namespace DisasterTrackerApp.BL.HttpClients.Contract;

public interface IGoogleOAuthHttpClient
{
    Task<AuthTokenResponse?> ExchangeCode(string code, CancellationToken cancellationToken);
    Task<RefreshAccessTokenResponse?> RefreshAccessToken(string refreshToken);
}