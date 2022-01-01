using System.Text.Json.Serialization;

namespace DisasterTrackerApp.Models.Disaster;

public class EventResponseDto
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("events")]
    public List<EventResponseDto> Events { get; set; }
}