using Newtonsoft.Json;

namespace DisasterTrackerApp.Models.Disaster
{
    public class PropertyDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("closed")]
        public DateTime Closed { get; set; }
        [JsonProperty("categories")]
        public List<CategoryDto?>? Categories { get; set; }
        [JsonProperty("sources")]
        public List<SourceDto?>? Sources { get; set; }
    }
}
