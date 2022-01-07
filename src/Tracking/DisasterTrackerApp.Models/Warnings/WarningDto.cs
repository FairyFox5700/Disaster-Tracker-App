namespace DisasterTrackerApp.Models.Warnings;

public record WarningDto(Guid CalendarId,
    Guid DisasterId,
    string Description,
    DateTimeOffset? EndTs,
    DateTimeOffset? StartTs)
{
    public Guid Id { get; init; } = Guid.NewGuid();
}
