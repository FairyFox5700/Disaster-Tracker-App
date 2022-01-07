using DisasterTrackerApp.Models.GoogleOAuth;

namespace DisasterTrackerApp.BL.HttpClients.Contract;

public interface IGoogleOAuthHttpClient
{
    Task<AccessTokenExchangeResponse?> ExchangeCode(string code, CancellationToken cancellationToken);
    Task<RefreshAccessTokenResponse?> RefreshAccessToken(string refreshToken);
}