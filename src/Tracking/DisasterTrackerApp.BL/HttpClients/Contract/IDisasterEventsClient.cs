using DisasterTrackerApp.Models.Disaster;

namespace DisasterTrackerApp.BL.HttpClients.Contract;

public interface IDisasterEventsClient
{
    IObservable<DisasterEventDto> GetDisasterEventsAsync(CancellationToken cancellationToken);
}