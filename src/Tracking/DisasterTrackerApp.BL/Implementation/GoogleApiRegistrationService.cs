using System.IdentityModel.Tokens.Jwt;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Utils.Extensions;

namespace DisasterTrackerApp.BL.Implementation;

public class GoogleApiAccessService : IGoogleApiAccessService
{
    private readonly IGoogleOAuthHttpClient _httpClient;
    private readonly IGoogleUserRepository _usersRepository;

    public GoogleApiAccessService(IGoogleOAuthHttpClient httpClient, IGoogleUserRepository usersRepository)
    {
        _httpClient = httpClient;
        _usersRepository = usersRepository;
    }
    
    public async Task<GoogleUser?> GetUserAccessCredentialsAsync(string authorizationCode, CancellationToken cancellationToken)
    {
        var userCredentials = await _httpClient.ExchangeCode(authorizationCode, cancellationToken);
        if (userCredentials is null)
        {
            return default;
        }

        var googleUser = new GoogleUser
        {
            UserGoogleId = GetUniqueUserGoogleId(userCredentials.IdToken),
            AccessToken = userCredentials.AccessToken,
            RefreshToken = userCredentials.RefreshToken,
            ExpiresIn = DateTimeOffsetHelper.CalculateTokenExpirationTime(userCredentials.ExpiresIn)
        };
        
        return googleUser;
    }

    public async Task<bool> TryRefreshAccessTokenForUserAsync(Guid userId, bool force = false)
    {
        var user = await _usersRepository.FindUserAsync(userId);
        if (user is null)
        {
            return false;
        }

        if (force == false && user.IsExpired == false)
        {
            return true;
        }
        
        var newAccessTokenResponse = await _httpClient.RefreshAccessToken(user.RefreshToken);
        if (newAccessTokenResponse is null)
        {
            return false;
        }
        
        await _usersRepository.UpdateAccessTokenAsync(user.UserId, newAccessTokenResponse.AccessToken, 
            newAccessTokenResponse.ExpiresIn);
        
        return true;
    }

    private string GetUniqueUserGoogleId(string jwtIdToken)
    {
        return new JwtSecurityTokenHandler().ReadJwtToken(jwtIdToken).Subject;
    }
}