using System.Text.Json.Serialization;

namespace DisasterTrackerApp.Models.Disaster;

public class GeometryDto
{
    [JsonPropertyName("magnitudeValue")]
    public double? MagnitudeValue { get; set; }

    [JsonPropertyName("magnitudeUnit")]
    public string MagnitudeUnit { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("coordinates")]
    public List<double> Coordinates { get; set; }
}