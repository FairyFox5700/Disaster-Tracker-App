namespace DisasterTracker.Identity.DAL.Entities;

public class ApplicationUser
{
    public  string UserId { get; set; }
    public  string AccessToken { get; set; }
    public  string RefreshToken { get; set; }
}