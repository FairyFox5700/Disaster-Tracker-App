using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface IGoogleUserRepository
{
    Task<List<GoogleUser>> GetGoogleUsersFilteredAsync(Expression<Func<GoogleUser, bool>> predicate);
    Task<GoogleUser?> FindUserAsync(Guid id);
    Task AddAsync(GoogleUser googleUser);
    Task UpdateAccessTokenAsync(Guid userId, string newAccessToken, long expiresIn);
    Task UpdateUserTokensAsync(Guid userId, string newAccessToken, DateTimeOffset expiresAt, string newRefreshToken);
    Task SetLastVisitDateTime(GoogleUser user);
}