using Newtonsoft.Json;

namespace DisasterTrackerApp.Models.Disaster
{
    public class FeatureCollection
    {
        [JsonProperty("features")]
        public List<FeatureDto> Features { get; set; }
    }
}
