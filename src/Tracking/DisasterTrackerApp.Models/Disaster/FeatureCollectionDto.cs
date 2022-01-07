using Newtonsoft.Json;

namespace DisasterTrackerApp.Models.Disaster
{
    public class FeatureCollectionDto
    {
        [JsonProperty("features")]
        public List<FeatureDto> Features { get; set; }
    }
}
