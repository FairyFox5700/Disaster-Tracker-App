namespace DisasterTrackerApp.Models.Disaster;

public class EventResponseDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public List<EventDto> Events { get; set; }
}