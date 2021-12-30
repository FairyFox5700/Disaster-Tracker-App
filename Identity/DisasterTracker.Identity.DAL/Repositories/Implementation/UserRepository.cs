using DisasterTracker.Identity.DAL.Entities;
using DisasterTracker.Identity.DAL.Repositories.Contract;

namespace DisasterTracker.Identity.DAL.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly IdentityContext _context;

    public UserRepository(IdentityContext context)
    {
        _context = context;
    }
    public async Task<ApplicationUser?> GetUserById(Guid userId)
    {
       return await _context.ApplicationUsers.FindAsync(userId);
    }
}