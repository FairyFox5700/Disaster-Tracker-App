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
        public IObservable<ApiResponse<List<FeatureDto>>> GetDisasterEventsAsync(CancellationToken cancellationToken)
        {
            return Observable.FromAsync(async token => await _httpClient
                    .SendAsync(new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress),
                        HttpCompletionOption.ResponseHeadersRead, token))
                .Select(async response => new
                {
                    Response = await response.Content.ReadAsStringAsync(cancellationToken),
                    StatusCode = response.StatusCode,
                    IsSucces = response.IsSuccessStatusCode,
                })
                .SelectMany(e => e)
                .Select(data =>
                {
                    var featureCollection = JsonConvert.DeserializeObject<FeatureCollectionDto>(data.Response);
                    if (featureCollection != null)
                    {
                        return new ApiResponse<List<FeatureDto>>
                        {
                            StatusCode = (int) data.StatusCode,
                            Data = featureCollection.Features
                        };
                    }
                    else
                    {
                        return new ApiResponse<List<FeatureDto>>
                        {
                            StatusCode = (int) data.StatusCode,
                            Data = new List<FeatureDto>(),
                            ResponseException = new ApiError(ErrorCode.InternalError, data.Response ?? ""),
                        };
                    }
                });

        }
    }
}
