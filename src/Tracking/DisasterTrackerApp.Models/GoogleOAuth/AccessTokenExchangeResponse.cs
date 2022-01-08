using Newtonsoft.Json;

namespace DisasterTrackerApp.Models.GoogleOAuth;

public class AccessTokenExchangeResponse : AccessTokenResponse
{
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
}