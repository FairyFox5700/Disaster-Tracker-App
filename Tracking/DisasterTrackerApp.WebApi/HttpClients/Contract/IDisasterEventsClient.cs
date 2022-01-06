using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.ApiModels.Base;
using DisasterTrackerApp.Models.Disaster;

namespace DisasterTrackerApp.WebApi.HttpClients.Contract;

public interface IDisasterEventsClient
{
    Task<ApiResponse<List<EventResponseDto>?>> GetDisasterEventsAsync(CancellationToken cancellationToken);
}