using DisasterTrackerApp.BL.Contract;
using Newtonsoft.Json;

namespace DisasterTrackerApp.BL.Implementation;

public class TempRedis : ITempRedis
{
    private Dictionary<string, string> _cache;

    public TempRedis()
    {
        _cache = new Dictionary<string, string>();
    }
    
    public void Set<T>(string key, T value)
    {
        if (_cache.ContainsKey(key))
        {
            _cache.Remove(key);
        }
        
        _cache.Add(key, JsonConvert.SerializeObject(value));
    }

    public T Get<T>(string key)
    {
        var value = _cache[key];
        return JsonConvert.DeserializeObject<T>(value);
    }
}