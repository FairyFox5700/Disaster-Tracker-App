namespace DisasterTrackerApp.Entities;

public class WatchChannelEntity
{
    public Guid UserId { get; set; }
    public string ChannelId { get; set; }
    public string ResourceId { get; set; }
    public DateTime LastTimeTriggered { get; set; }
}