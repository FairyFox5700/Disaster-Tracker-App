using Newtonsoft.Json;

namespace DisasterTrackerApp.Models.Auth;

    public class AuthTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("scope")] 
        public  string Scope { get; set; }
        [JsonProperty("expires_in")]
        public  long ExpiresIn { get; set; }
    }