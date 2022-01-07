namespace DisasterTrackerApp.Models.Configuration;

public class GoogleWebHookOptions
{
    public const string Section = "GoogleWebHook";
    
    public string Address { get; set; }
    public string Url { get; set; }
}