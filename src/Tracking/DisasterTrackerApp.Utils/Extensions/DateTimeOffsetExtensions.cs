namespace DisasterTrackerApp.Utils.Extensions;

public static class DateTimeOffsetHelper
{
    public static DateTimeOffset CalculateTokenExpirationTime(long seconds)
    {
        return DateTimeOffset.UtcNow.AddSeconds(seconds);
    }
}