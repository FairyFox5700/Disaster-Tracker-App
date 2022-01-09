using NetTopologySuite;
using NetTopologySuite.IO;
using Newtonsoft.Json;

namespace DisasterTrackerApp.Dal.Extensions;

public static class JsonExtensions
{
    public static T? GeoJsonDeserialize<T>(string item, JsonSerializerSettings? settings = null)
    {
        var jsonSerializer = GeoJsonSerializer.Create(new JsonSerializerSettings(),
            NtsGeometryServices.Instance.CreateGeometryFactory(4326));
        return jsonSerializer.Deserialize<T>(
            new JsonTextReader(new StringReader(item)));
    }

    public static string GeoJsonSerialize<T>(T item, JsonSerializerSettings? settings = null)
    {
        var jsonSerializer = GeoJsonSerializer.Create(new JsonSerializerSettings(),
            NtsGeometryServices.Instance.CreateGeometryFactory(4326));
        var stringBuilder = new System.Text.StringBuilder();
        jsonSerializer.Serialize(new JsonTextWriter(new StringWriter(stringBuilder)), item);
        return stringBuilder.ToString();
    }
}
