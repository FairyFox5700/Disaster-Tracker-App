using System.Linq.Expressions;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface IDisasterEventRepository
{
    Task<DisasterEvent?> GetDisasterEventByExternalId(string externalId);
    Task<List<DisasterEvent>> GetDisasterEventsFiltered(Expression<Func<DisasterEvent,bool>> predicate);
}