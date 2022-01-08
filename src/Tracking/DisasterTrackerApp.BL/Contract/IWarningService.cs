using DisasterTrackerApp.Models.Warnings;

namespace DisasterTrackerApp.BL.Contract;

public interface IWarningService
{
    IObservable<WarningDto> GetWarningEvents(WarningRequest warningRequest,
        CancellationToken cancellationToken = default);
    IObservable<WarningDto> GetStatisticsWarningEvents(WarningRequest warningRequest,
            CancellationToken cancellationToken = default);
}