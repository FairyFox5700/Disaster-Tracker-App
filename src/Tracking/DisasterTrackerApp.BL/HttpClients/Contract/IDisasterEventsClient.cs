using DisasterTrackerApp.Models.Disaster;

namespace DisasterTrackerApp.BL.HttpClients.Contract;

public interface IDisasterEventsClient
{
    IObservable<FeatureDto> GetDisasterEventsAsync(CancellationToken cancellationToken);
}