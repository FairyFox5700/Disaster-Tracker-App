using Newtonsoft.Json;

namespace DisasterTrackerApp.Models.GoogleOAuth;

public class AccessTokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("scope")] 
    public string Scope { get; set; }
        
    [JsonProperty("expires_in")]
    public long ExpiresIn { get; set; }
    
    [JsonProperty("id_token")]
    public string IdToken { get; set; }
}