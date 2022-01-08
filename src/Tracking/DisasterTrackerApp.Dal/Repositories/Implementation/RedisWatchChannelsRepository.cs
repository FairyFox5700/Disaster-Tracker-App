using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class RedisWatchChannelsRepository : IRedisWatchChannelsRepository
{
    private const string WatchChannelsHashSet = "WatchChannelsHashSet";
    private const string UserChannelHashSet = "UserChannelHashSet";

    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisWatchChannelsRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public WatchChannelEntity? GetWatchChannel(string channelToken)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var hashGet = db.HashGet(WatchChannelsHashSet, channelToken);

        return !string.IsNullOrEmpty(hashGet) ? JsonConvert.DeserializeObject<WatchChannelEntity>(hashGet) : null;
    }
    
    public WatchChannelEntity? GetWatchChannel(Guid userId)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var channelToken = db.HashGet(UserChannelHashSet, userId.ToString()).ToString();
        if (string.IsNullOrEmpty(channelToken))
        {
            return null;
        }
        
        var hashGet = db.HashGet(WatchChannelsHashSet, channelToken);

        return !string.IsNullOrEmpty(hashGet) ? JsonConvert.DeserializeObject<WatchChannelEntity>(hashGet) : null;
    }

    public string? GetChannelToken(Guid userId)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var hashGet = db.HashGet(UserChannelHashSet, userId.ToString());

        return !string.IsNullOrEmpty(hashGet) ? hashGet.ToString() : null;
    }
    
    public void RemoveWatchChannel(Guid userId)
    {
        var db = _connectionMultiplexer.GetDatabase();
        
        var channelToken = db.HashGet(UserChannelHashSet, userId.ToString()).ToString();
        if (string.IsNullOrEmpty(channelToken))
        {
            return;
        }
        
        db.HashDelete(UserChannelHashSet, userId.ToString());
        db.HashDelete(WatchChannelsHashSet, channelToken);
    }

    public void Save(string channelToken, WatchChannelEntity? watchChannelEntity)
    {
        if (watchChannelEntity == null)
        {
            throw new ArgumentOutOfRangeException(nameof(watchChannelEntity));
        }

        var data = JsonConvert.SerializeObject(watchChannelEntity);

        var db = _connectionMultiplexer.GetDatabase();
        SaveData(db, WatchChannelsHashSet, channelToken, data);
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