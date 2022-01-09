using System.Reactive.Linq;
using DisasterTrackerApp.Dal.Extensions;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DisasterTrackerApp.Dal.Repositories.Implementation;

public class RedisDisasterEventsRepository:IRedisDisasterEventsRepository
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisDisasterEventsRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }
    
    public void CreateDisasterEvent(DisasterEvent disasterEvent)
    {
        if (disasterEvent == null)
        {
            throw new ArgumentOutOfRangeException(nameof(disasterEvent));
        }

        var db = _connectionMultiplexer.GetDatabase();

        var serializedDisasterEvent =JsonExtensions.GeoJsonSerialize(disasterEvent);
        db.HashSet($"hashDisasterEvent", 
            new HashEntry[] 
            {
                new(new RedisValue(disasterEvent.ExternalApiId), 
                new RedisValue(serializedDisasterEvent))
            });
    }
    public IObservable<DisasterEvent?> GetDisasterEventById(string id)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var hashGet = db.HashGet("hashDisasterEvent", id);

        return !string.IsNullOrEmpty(hashGet) ?
            Observable.Return(JsonExtensions.GeoJsonDeserialize<DisasterEvent>(hashGet))
            : Observable.Empty((DisasterEvent)null);
    }

    public IObservable<List<DisasterEvent?>> GetAllDisasterEvents()
    {
        var db = _connectionMultiplexer.GetDatabase();

        var completeSet = db.HashGetAll("hashDisasterEvent");

        if (completeSet.Length <= 0) return Observable.Empty(new List<DisasterEvent?>());
        var disasters = Array.ConvertAll(completeSet, val => 
                JsonExtensions.GeoJsonDeserialize<DisasterEvent>(val.Value))
            .ToList();
        return Observable.Return(disasters);

    }
}