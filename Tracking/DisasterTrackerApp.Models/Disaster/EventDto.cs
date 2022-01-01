using System.Text.Json.Serialization;

namespace DisasterTrackerApp.Models.Disaster;

public class EventDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("closed")]
    public DateTime? Closed { get; set; }

    [JsonPropertyName("categories")]
    public List<CategoryDto> Categories { get; set; }

    [JsonPropertyName("sources")]
    public List<SourceDto> Sources { get; set; }

    [JsonPropertyName("geometry")]
    public List<GeometryDto> Geometry { get; set; }
}