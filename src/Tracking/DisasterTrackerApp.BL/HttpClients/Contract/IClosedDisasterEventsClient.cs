using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.ApiModels.Base;
using DisasterTrackerApp.Models.Disaster;

namespace DisasterTrackerApp.BL.HttpClients.Contract
{
    public interface IClosedDisasterEventsClient
    {
        Task<ApiResponse<List<FeatureDto>>> GetDisasterEventsAsync(CancellationToken cancellationToken);
    }
}
