using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.ApiModels;
using DisasterTrackerApp.Models.Disaster;
using DisasterTrackerApp.Utils.Extensions;
using DisasterTrackerApp.WebApi.HttpClients.Contract;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace DisasterTrackerApp.WebApi.HttpClients.Implementation;

public class DisasterEventsClient :IDisasterEventsClient
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
        _httpClient.DefaultRequestHeaders.Add("User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 Edg/96.0.1054.62");
    }
    
    public IObservable<DisasterEventDto> GetDisasterEventsAsync(CancellationToken cancellationToken)
    {
        var connectionUri = new Uri(EventsGetUrl);
        return Observable.Interval(TimeSpan.FromMinutes(100))
            .SelectMany(e => _httpClient.GetAsync(connectionUri, cancellationToken))
            .Do(e => _logger.LogInformation("[EXTERNAL-SERVICE] Connecting to EONET service.com ..."))
            .SelectMany(e => e.Content.ReadAsStringAsync(cancellationToken))
            .Select(JsonConvert.DeserializeObject<EventResponseDto>)
            .SelectMany(e => e?.Events ?? new List<DisasterEventDto>())
            .Do(e => Console.WriteLine(e.Id))
            .Do(e=>
            {
                if (_redisDisasterEventsRepository.GetDisasterEventById(e.Id) == null)
                {
                    _redisDisasterEventsRepository.CreateDisasterEvent((new DisasterEvent()
                    {
                        Description = e.Description??"",
                        Active = e.Closed != null,
                        CategoryTittle = e.Categories.First()?.Title,
                        Coordiantes = new Point(e.Geometry.First().Coordinates[0], e.Geometry.First().Coordinates[1]),
                        ExternalApiId = e.Id,
                        Tittle = e.Title,
                        CreatedAt = DateTimeOffset.Now,
                        Id =Guid.NewGuid(),
                        UpdatedAt = DateTimeOffset.Now,
                    }));
                }
            })
            .DistinctUntilChanged(e => e.Id);
    }
}