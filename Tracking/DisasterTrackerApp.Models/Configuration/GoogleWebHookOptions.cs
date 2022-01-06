namespace DisasterTrackerApp.Models.Configuration;

public class GoogleWebHookOptions
{
    public static string Section = "GoogleWebHook";
    
    public string WebHookUrl { get; set; }   
}