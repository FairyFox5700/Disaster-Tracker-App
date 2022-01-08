namespace DisasterTrackerApp.Models.Configuration;

public class GoogleWebHookOptions
{
    public const string Section = "GoogleWebHook";
    
    public string Address { get; set; } = null!;
    public string Url { get; set; } = null!;
}