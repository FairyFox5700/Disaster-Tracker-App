using System.Reactive;
using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface IRedisDisasterEventsRepository
{
    void CreateDisasterEvent(DisasterEvent disasterEvent);
    IObservable<DisasterEvent?> GetDisasterEventById(string id);
    IObservable<List<DisasterEvent?>> GetAllDisasterEvents();
}