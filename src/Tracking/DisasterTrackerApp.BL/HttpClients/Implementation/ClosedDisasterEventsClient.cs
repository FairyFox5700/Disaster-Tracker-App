using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.Disaster;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.Models.ApiModels.Base;
using Newtonsoft.Json;

namespace DisasterTrackerApp.BL.HttpClients.Implementation
{
    public class ClosedDisasterEventsClient : IClosedDisasterEventsClient
    {
        private readonly HttpClient _httpClient;

        public ClosedDisasterEventsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<FeatureDto>>> GetDisasterEventsAsync(CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress);
            using var response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
            var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var featureCollection = JsonConvert.DeserializeObject<FeatureCollectionDto>(jsonResponse);
                if (featureCollection != null)
                {
                    return new ApiResponse<List<FeatureDto>>
                    {
                        StatusCode = (int) response.StatusCode,
                        Data = featureCollection.Features
                    };
                }
            }

            return new ApiResponse<List<FeatureDto>>
            {
                StatusCode = (int) response.StatusCode,
                Data = new List<FeatureDto>(),
                ResponseException = new ApiError(ErrorCode.InternalError, jsonResponse ?? ""),
            };
        }
    }
}
