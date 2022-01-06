using Newtonsoft.Json;

namespace DisasterTrackerApp.Models.GoogleOAuth;

    public class AuthTokenResponse : AccessTokenResponse
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }