namespace DisasterTrackerApp.Models.Calendar;

public class WatchChannelData
{
    public Guid UserId { get; set; }
    public string ChannelId { get; set; }
    public string ResourceId { get; set; }
    public DateTime LastTimeTriggered { get; set; }

    public WatchChannelData(Guid userId, string channelId, string resourceId, DateTime? triggerTime = null)
    {
        UserId = userId;
        ChannelId = channelId;
        ResourceId = resourceId;
        LastTimeTriggered = triggerTime ?? DateTime.UtcNow;
    }

    public WatchChannelData UpdateTriggerTime(DateTime lastTriggerTime)
    {
        LastTimeTriggered = lastTriggerTime;

        return this;
    }
}
