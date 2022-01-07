using Newtonsoft.Json;

namespace DisasterTrackerApp.Models.Disaster;

public class CategoryDto
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
}