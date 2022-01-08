namespace DisasterTrackerApp.Models.Warnings;

public record WarningRequest(
    Guid UserId,
    DateTimeOffset? EndDateTimeOffset,
    DateTimeOffset? StartDateTimeOffset);