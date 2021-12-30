namespace DisasterTrackerApp.Models.Disaster;

public class GeometryDto
{
    public double? MagnitudeValue { get; set; }
    public string MagnitudeUnit { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; }
    public List<double> Coordinates { get; set; }
}