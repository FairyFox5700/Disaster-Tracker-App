using Newtonsoft.Json;

namespace DisasterTrackerApp.Utils.Extensions;

public static class HttpClientExtensions
{
    public static T? DeserializeJsonFromStream<T>(Stream? stream)
    {
        if (stream == null || stream.CanRead == false)
            return default(T);

        using var sr = new StreamReader(stream);
        using var jtr = new JsonTextReader(sr);
        var js = new JsonSerializer();
        var searchResult = js.Deserialize<T>(jtr);
        return searchResult;
    }
    
    public static async Task<string?> StreamToStringAsync(Stream? stream)
    {
        string? content = null;

        if (stream == null) return content;
        using var sr = new StreamReader(stream);
        content = await sr.ReadToEndAsync();

        return content;
    }
}