using System.Text.Json.Serialization;

namespace DisasterTrackerApp.Models.Disaster;

public class SourceDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}