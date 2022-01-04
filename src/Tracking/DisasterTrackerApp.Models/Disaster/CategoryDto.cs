using System.Text.Json.Serialization;

namespace DisasterTrackerApp.Models.Disaster;

public class CategoryDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
}