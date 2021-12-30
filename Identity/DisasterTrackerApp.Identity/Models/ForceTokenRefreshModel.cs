namespace DisasterTrackerApp.Identity.Controllers;

    public record ForceTokenRefreshModel
    {
        public IReadOnlyList<string> Results;
        public string AccessToken;
    }