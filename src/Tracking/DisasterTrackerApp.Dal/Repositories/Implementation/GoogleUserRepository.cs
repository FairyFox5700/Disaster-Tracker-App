using System.Linq.Expressions;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8613

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class GoogleUserRepository : IGoogleUserRepository
{
    private readonly DisasterTrackerContext _context;

    public GoogleUserRepository(DisasterTrackerContext context)
    {
        _context = context;
    }
    
    public async Task<List<GoogleUser>> GetGoogleUsersFilteredAsync(Expression<Func<GoogleUser, bool>> predicate)
    {
        return await _context.GoogleUsers.Where(predicate).ToListAsync();
    }

    public async Task<GoogleUser?> FindUserAsync(Guid id)
    {
        return await _context.GoogleUsers.FindAsync(id);
    }

    public async Task AddAsync(GoogleUser googleUser)
    {
        await _context.GoogleUsers.AddAsync(googleUser);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAccessTokenAsync(Guid userId, string newAccessToken, long expiresIn)
    {
        var user = await _context.GoogleUsers.FindAsync(userId);
        if (user == null)
        {
            return;
        }
        
        user.AccessToken = newAccessToken;
        user.ExpiresIn = DateTimeOffsetHelper.CalculateTokenExpirationTime(expiresIn);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateTokensAsync(Guid userId, string newAccessToken, DateTimeOffset expiresAt, string newRefreshToken)
    {
        var user = await _context.GoogleUsers.FindAsync(userId);
        if (user == null)
        {
            return;
        }
        
        user.AccessToken = newAccessToken;
        user.ExpiresIn = expiresAt;
        user.RefreshToken = newRefreshToken;
        await _context.SaveChangesAsync();
    }

    public async Task SetLastLoginDataUpdateDateTime(GoogleUser user)
    {
        user.LastVisit = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}