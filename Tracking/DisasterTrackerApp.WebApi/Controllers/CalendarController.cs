using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DisasterTrackerApp.Identity.Controllers;

public class CalendarController:ControllerBase
{
    private readonly HttpClient _httpClient;

    public CalendarController(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("Accept","application/json");
        _httpClient.DefaultRequestHeaders.Add("User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 Edg/96.0.1054.62");
    }

    [HttpGet("/receive")]
    public async Task ReceveEvents(CancellationToken cancellationToken, string apiKey, string state)
    {
        var response = Response;
        response.Headers.Add("Content-Type", "text/event-stream");
        
        await Observable.Interval(TimeSpan.FromSeconds(1))
            .SelectMany(e => _httpClient.GetAsync("https://eonet.gsfc.nasa.gov/api/v3/events",cancellationToken))
            .SelectMany(e => e.Content.ReadAsStringAsync(cancellationToken))
            .Select(JsonConvert.DeserializeObject<Root>)
            .SelectMany(e => e?.events ?? new List<Event>())
            .SelectMany(async e =>
            {
                await response.WriteAsync($"{JsonConvert.SerializeObject(e)}\r\r", cancellationToken: cancellationToken);
                await response.Body.FlushAsync(cancellationToken);
                return e;
            })
            .ToTask(cancellationToken);
    }

    
    public class Category
    {
        public string id { get; set; }
        public string title { get; set; }
    }

    public class Source
    {
        public string id { get; set; }
        public string url { get; set; }
    }

    public class Geometry
    {
        public double? magnitudeValue { get; set; }
        public string magnitudeUnit { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Event
    {
        public string id { get; set; }
        public string title { get; set; }
        public object description { get; set; }
        public string link { get; set; }
        public object closed { get; set; }
        public List<Category> categories { get; set; }
        public List<Source> sources { get; set; }
        public List<Geometry> geometry { get; set; }
    }

    public class Root
    {
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public List<Event> events { get; set; }
    }
}