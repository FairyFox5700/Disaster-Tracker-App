using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.Disaster;
using DisasterTrackerApp.Utils.Extensions;
using DisasterTrackerApp.BL.HttpClients.Contract;
using Newtonsoft.Json;

namespace DisasterTrackerApp.BL.HttpClients.Implementation
{
    public class ClosedDisasterEventsClient:IClosedDisasterEventsClient
    {
        private readonly HttpClient _httpClient;
        public ClosedDisasterEventsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResponse<List<FeatureDto?>?>> GetDisasterEventsAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ApiResponse<List<FeatureDto?>?>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = JsonConvert.DeserializeObject<FeatureCollection>(jsonResponse).Features
                };

            return new ApiResponse<List<FeatureDto?>?>
            {
                StatusCode = (int)response.StatusCode,
                Data = new List<FeatureDto?>(),
                ResponseException = new ApiError(ErrorCode.InternalError, jsonResponse ?? ""),
            };
        }
    }
}
