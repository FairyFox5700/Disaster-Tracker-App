using System.Linq.Expressions;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.BL.Implementation;

public class RegistrationService : IRegistrationService
{
    private readonly IGoogleApiAccessService _apiAccessService;
    private readonly IUsersGoogleCalendarDataUpdatingService _updatingService;
    private readonly IGoogleUserRepository _userRepository;

    public RegistrationService(
        IGoogleApiAccessService apiAccessService,
        IUsersGoogleCalendarDataUpdatingService updatingService, 
        IGoogleUserRepository userRepository)
    {
        _apiAccessService = apiAccessService;
        _updatingService = updatingService;
        _userRepository = userRepository;
    }
    
    public async Task<Guid> RegisterUser(string authorizationCode, CancellationToken cancellationToken)
    {
        var user = await _apiAccessService.GetUserAccessCredentialsAsync(authorizationCode, cancellationToken);
        if (user == default)
        {
            return default;
        }

        if (await GetUser(u => u.UserGoogleId == user.UserGoogleId) != null)
        {
           await UpdateUser(user);
           return default;
        }

        await SaveNewUser(user);
        
        return user.UserId;
    }

    public async Task<string?> LoginUser(Guid userId)
    {
        var activeUser = await GetUser(u => u.UserId.Equals(userId));
        if (activeUser == null)
        {
            return null;
        }
        
        var tokenRefreshed = await _apiAccessService.TryRefreshAccessTokenForUserAsync(activeUser.UserId);
        if (tokenRefreshed == false)
        {
            return null;
        }
        
        await _updatingService.UpdateUserData(activeUser);

        var watchToken = await _updatingService.RegisterWatch(userId);
        
        return watchToken;
    }
    
    private async Task SaveNewUser(GoogleUser user)
    {
        await _userRepository.AddAsync(user);
    }
    
    private async Task UpdateUser(GoogleUser user)
    {
        var existingUser = (await _userRepository.GetGoogleUsersFilteredAsync(
                u => u.UserGoogleId.Equals(user.UserGoogleId)))
            .FirstOrDefault();
        
        if (existingUser == null)
        {
            return;
        }

        await _userRepository.UpdateTokensAsync(existingUser.UserId, user.AccessToken, 
            user.ExpiresIn, user.RefreshToken);
    }
    
    private async Task<GoogleUser?> GetUser(Expression<Func<GoogleUser, bool>> filter)
    {
        return (await _userRepository.GetGoogleUsersFilteredAsync(filter)).FirstOrDefault();
    }
}