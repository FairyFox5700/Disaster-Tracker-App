using DisasterTrackerApp.Entities;

namespace DisasterTrackerApp.Dal.Repositories.Contract;

public interface IRedisWatchChannelsRepository
{
    WatchChannelEntity? GetWatchChannel(string channelToken);
    WatchChannelEntity? GetWatchChannel(Guid userId);
    string? GetChannelToken(Guid userId);
    void Save(string channelToken, WatchChannelEntity? watchChannelEntity);
    void RemoveWatchChannel(Guid userId);
}