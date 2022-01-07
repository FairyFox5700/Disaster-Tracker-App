using Newtonsoft.Json;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;

namespace DisasterTrackerApp.Models.Disaster
{
    public class FeatureDto
    {
        [JsonProperty("properties")]
        public PropertyDto Properties { get; set; }
        [JsonProperty("geometry")]
        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Geometry { get; set; }
    }
}
