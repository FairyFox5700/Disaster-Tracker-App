namespace DisasterTrackerApp.Models.GoogleOAuth;

public class AuthenticationToken
{
   public  string AccessToken { get; set; }
   public  string RefreshToken { get; set; }
   public  long ExpiresAt { get; set; }
}