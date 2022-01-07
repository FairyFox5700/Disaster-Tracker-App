namespace DisasterTrackerApp.Models.Configuration;

public class GoogleWebHookOptions
{
    public static string Section = "GoogleWebHook";
    
    public string Address { get; set; }
    public string Url { get; set; }
}