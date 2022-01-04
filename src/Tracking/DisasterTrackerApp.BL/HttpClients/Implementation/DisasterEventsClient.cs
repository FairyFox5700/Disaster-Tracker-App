using System.Reactive.Linq;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Models.Disaster;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DisasterTrackerApp.BL.HttpClients.Implementation;

public partial class DisasterEventsClient :IDisasterEventsClient
{
    private readonly HttpClient _httpClient;
    private readonly IRedisDisasterEventsRepository _redisDisasterEventsRepository;
    private readonly string EventsGetUrl = "https://eonet.gsfc.nasa.gov/api/v3/events";
    private readonly ILogger<DisasterEventsClient> _logger;

    public DisasterEventsClient(HttpClient httpClient,
        IRedisDisasterEventsRepository redisDisasterEventsRepository,
        ILogger<DisasterEventsClient> logger)
    {
        _httpClient = httpClient;
        _redisDisasterEventsRepository = redisDisasterEventsRepository;
        _logger = logger;
        _httpClient.DefaultRequestHeaders.Add("Accept","application/json");
        _httpClient.DefaultRequestHeaders
            .Add("User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 Edg/96.0.1054.62");

    }
    
    public IObservable<DisasterEventDto> GetDisasterEventsAsync(CancellationToken cancellationToken)
    {
        return Observable.Interval(TimeSpan.FromSeconds(1))
            .SelectMany(e => _httpClient.GetAsync(EventsGetUrl, cancellationToken))
            .Do(e => _logger.LogInformation("[EXTERNAL-SERVICE] Connecting to EONET service.com ..."))
            .SelectMany(e => e.Content.ReadAsStringAsync(cancellationToken))
            .Select(JsonConvert.DeserializeObject<EventResponseDto>)
            .SelectMany(e => e?.Events ?? new List<DisasterEventDto>())
            .Do(e => Console.WriteLine(e.Id));

    }
}