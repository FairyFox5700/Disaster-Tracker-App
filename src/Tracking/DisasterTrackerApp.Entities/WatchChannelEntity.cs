namespace DisasterTrackerApp.Entities;

public class WatchChannelEntity
{
    public string ChannelToken { get; set; } = null!;
    public string ChannelId { get; set; } = null!;
    public Guid UserId { get; set; }
    public string ResourceId { get; set; } = null!;
    public DateTime LastTimeTriggered { get; set; }
}