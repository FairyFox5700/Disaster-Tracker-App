namespace DisasterTrackerApp.Entities;

public class CalendarEvent
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string GoogleId { get; set; }
    public string Summary { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Location { get; set; }
    public Coordiantes Coordiantes { get; set; }
}