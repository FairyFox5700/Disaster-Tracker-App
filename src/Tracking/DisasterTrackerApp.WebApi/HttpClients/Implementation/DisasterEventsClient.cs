using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.Disaster;
using DisasterTrackerApp.Utils.Extensions;
using DisasterTrackerApp.WebApi.HttpClients.Contract;

namespace DisasterTrackerApp.WebApi.HttpClients.Implementation;

public class DisasterEventsClient :IDisasterEventsClient
{
    private readonly HttpClient _httpClient;
    private readonly string EventsGetUrl = "events/json";
    public DisasterEventsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<ApiResponse<List<EventResponseDto>?>> GetDisasterEventsAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, EventsGetUrl);
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
            return new ApiResponse<List<EventResponseDto>?>
            {
                StatusCode = (int)response.StatusCode,
                Data = HttpClientExtensions.DeserializeJsonFromStream<List<EventResponseDto>>(stream),
            };

        var content = await HttpClientExtensions.StreamToStringAsync(stream);
        return new ApiResponse<List<EventResponseDto>?>
        {
            StatusCode = (int)response.StatusCode,
            Data = new List<EventResponseDto>(),
            ResponseException = new ApiError(ErrorCode.InternalError,content??""),
        };
    }
    
}