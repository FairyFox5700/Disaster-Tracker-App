using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class RedisWatchChannelsRepository : IRedisWatchChannelsRepository
{
    private const string ChannelDataHashSet = "hashTokenWatchChannel";
    private const string UserChannelHashSet = "hashUserChannel";

    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisWatchChannelsRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public WatchChannelEntity? GetWatchChannel(string channelToken)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var hashGet = db.HashGet(ChannelDataHashSet, channelToken);

        return !string.IsNullOrEmpty(hashGet) ? JsonConvert.DeserializeObject<WatchChannelEntity>(hashGet) : null;
    }

    public string? GetChannelToken(Guid userId)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var hashGet = db.HashGet(UserChannelHashSet, userId.ToString());

        return !string.IsNullOrEmpty(hashGet) ? hashGet.ToString() : null;
    }

    public void Save(string channelToken, WatchChannelEntity watchChannelEntity)
    {
        if (watchChannelEntity == null)
        {
            throw new ArgumentOutOfRangeException(nameof(watchChannelEntity));
        }

        var data = JsonConvert.SerializeObject(watchChannelEntity);

        var db = _connectionMultiplexer.GetDatabase();
        SaveData(db, ChannelDataHashSet, channelToken, data);
        SaveData(db, UserChannelHashSet, watchChannelEntity.UserId.ToString(), channelToken);
    }

    private void SaveData(IDatabase db, string hashSet, string key, string value)
    {
        db.HashSet(hashSet,
            new HashEntry[]
            {
                new(new RedisValue(key),
                    new RedisValue(value))
            });
    }
}