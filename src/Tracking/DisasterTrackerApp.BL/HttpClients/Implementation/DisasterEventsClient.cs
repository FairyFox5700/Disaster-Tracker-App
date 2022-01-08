using System.Reactive.Linq;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Models.Disaster;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DisasterTrackerApp.BL.HttpClients.Implementation;

public class DisasterEventsClient :IDisasterEventsClient
{
    private readonly HttpClient _httpClient;
    private readonly IRedisDisasterEventsRepository _redisDisasterEventsRepository;
    private readonly ILogger<DisasterEventsClient> _logger;

    public DisasterEventsClient(HttpClient httpClient,
        IRedisDisasterEventsRepository redisDisasterEventsRepository,
        ILogger<DisasterEventsClient> logger)
    {
        _httpClient = httpClient;
        _redisDisasterEventsRepository = redisDisasterEventsRepository;
        _logger = logger;

    }
    
    public IObservable<FeatureDto> GetDisasterEventsAsync(CancellationToken cancellationToken)
    {
        return Observable.Interval(TimeSpan.FromSeconds(10))
            .SelectMany(e => _httpClient.GetAsync(_httpClient.BaseAddress, cancellationToken))
            .Do(e => _logger.LogInformation("[EXTERNAL-SERVICE] Connecting to EONET service.com ..."))
            .SelectMany(e => e.Content.ReadAsStringAsync(cancellationToken))
            .Select(JsonConvert.DeserializeObject<FeatureCollectionDto>)
            .SelectMany(e => e.Features);
    }
}