using Newtonsoft.Json;

namespace DisasterTrackerApp.Models.Disaster;

public class SourceDto
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("url")]
    public string Url { get; set; }
}