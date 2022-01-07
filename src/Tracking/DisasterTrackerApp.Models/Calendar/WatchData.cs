namespace DisasterTrackerApp.Models.Calendar;

public class WatchData
{
    public Guid UserId { get; set; }
    public string ChannelId { get; set; }
    public string ResourceId { get; set; }
    public DateTime LastTimeTriggered { get; set; }

    public WatchData(Guid userId, string channelId, string resourceId, DateTime? triggerTime = null)
    {
        UserId = userId;
        ChannelId = channelId;
        ResourceId = resourceId;
        LastTimeTriggered = triggerTime ?? DateTime.UtcNow;
    }

    public WatchData UpdateTriggerTime(DateTime lastTriggerTime)
    {
        LastTimeTriggered = lastTriggerTime;

        return this;
    }
}
