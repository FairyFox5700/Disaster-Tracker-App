namespace DisasterTrackerApp.BL.Contract;

public interface ITempRedis
{
    void Set<T>(string key, T value);
    T Get<T>(string key);
}