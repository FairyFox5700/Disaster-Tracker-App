namespace DisasterTrackerApp.Models.Warnings;

public record WarningRequest(
    string UserId,
    DateTimeOffset? EndDateTimeOffset,
    DateTimeOffset? StartDateTimeOffset);