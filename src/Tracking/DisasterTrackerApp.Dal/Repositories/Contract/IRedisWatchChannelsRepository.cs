using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface IRedisWatchChannelsRepository
{
    WatchChannelEntity? GetWatchChannel(string channelToken);
    string? GetChannelToken(Guid userId);
    void Save(string channelToken, WatchChannelEntity watchChannelEntity);
}