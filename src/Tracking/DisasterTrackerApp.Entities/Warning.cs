namespace DisasterTrackerApp.Entities;

public class Warning
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CalendarEventId { get; set; }
    public int DisasterEventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UUID { get; set; } = null!;
}