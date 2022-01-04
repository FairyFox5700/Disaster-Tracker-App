using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface IRedisDisasterEventsRepository
{
    void CreateDisasterEvent(DisasterEvent disasterEvent);
    DisasterEvent? GetDisasterEventById(string id);
    IEnumerable<DisasterEvent?>? GetAllDisasterEvents();
}