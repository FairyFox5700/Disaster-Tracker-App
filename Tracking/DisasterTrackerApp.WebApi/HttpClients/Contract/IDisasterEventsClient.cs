using System.Reactive;
using System.Reactive.Subjects;
using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.Disaster;

namespace DisasterTrackerApp.WebApi.HttpClients.Contract;

public interface IDisasterEventsClient
{
    IObservable<DisasterEventDto> GetDisasterEventsAsync(CancellationToken cancellationToken);
}