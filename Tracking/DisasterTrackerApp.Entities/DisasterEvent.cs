namespace DisasterTrackerApp.Entities;

public class DisasterEvent
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; }
    public string Desription { get; set; }
    public bool Active { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public  Coordiantes Coordiantes { get; set; }
}