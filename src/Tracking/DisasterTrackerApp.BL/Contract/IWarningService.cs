using DisasterTrackerApp.Models.Warnings;

namespace DisasterTrackerApp.BL.Contract;

public interface IWarningService
{
    IObservable<WarningDto> GetWarningEvents(WarningRequest warningRequest,
        CancellationToken cancellationToken = default);
}