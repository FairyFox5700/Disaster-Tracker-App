using DisasterTracker.Identity.DAL.Entities;

namespace DisasterTracker.Identity.DAL.Repositories.Contract;

public interface IUserRepository
{
    public  Task<ApplicationUser?> GetUserById(Guid userId);
}