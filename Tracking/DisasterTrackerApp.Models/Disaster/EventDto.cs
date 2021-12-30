namespace DisasterTrackerApp.Models.Disaster;

public class EventDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public DateTime? Closed { get; set; }
    public List<CategoryDto> Categories { get; set; }
    public List<SourceDto> Sources { get; set; }
    public List<GeometryDto> Geometry { get; set; }
}