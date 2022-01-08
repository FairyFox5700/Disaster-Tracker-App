namespace DisasterTrackerApp.Entities;

public class WatchChannelEntity
{
    public string ChannelToken { get; set; }
    public string ChannelId { get; set; }
    public Guid UserId { get; set; }
    public string ResourceId { get; set; }
    public DateTime LastTimeTriggered { get; set; }
}