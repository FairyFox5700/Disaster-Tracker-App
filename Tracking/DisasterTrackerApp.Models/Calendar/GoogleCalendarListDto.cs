using System.Text.Json.Serialization;

namespace DisasterTrackerApp.Models.Calendar;

public record DefaultReminderDto
{
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    [JsonPropertyName("minutes")]
    public int? Minutes { get; set; }
}

public record ConferencePropertiesDto
{
    [JsonPropertyName("allowedConferenceSolutionTypes")]
    public List<string>? AllowedConferenceSolutionTypes { get; set; }

}

public record NotificationDto
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("method")]
    public string? Method { get; set; }
}

public class NotificationSettingsDto
{
    [JsonPropertyName("notifications")]
    public List<NotificationDto>? Notifications { get; set; }
}

public record CalendarDetailsDto
{
    [JsonPropertyName("kind")]
    public string? Kind { get; set; }

    [JsonPropertyName("etag")]
    public string Etag { get; set; }

    [JsonPropertyName("id")] 
    public string Id { get; set; } = null!;

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("timeZone")]
    public string? TimeZone { get; set; }

    [JsonPropertyName("colorId")]
    public string? ColorId { get; set; }

    [JsonPropertyName("backgroundColor")]
    public string? BackgroundColor { get; set; }

    [JsonPropertyName("foregroundColor")]
    public string? ForegroundColor { get; set; }

    [JsonPropertyName("selected")]
    public bool? Selected { get; set; }

    [JsonPropertyName("accessRole")]
    public string? AccessRole { get; set; }

    [JsonPropertyName("defaultReminders")]
    public List<DefaultReminderDto>? DefaultReminders { get; set; }

    [JsonPropertyName("conferenceProperties")]
    public ConferencePropertiesDto? ConferenceProperties { get; set; }

    [JsonPropertyName("notificationSettings")]
    public NotificationSettingsDto? NotificationSettings { get; set; }

    [JsonPropertyName("primary")]
    public bool? Primary { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

public record GoogleCalendarListDto
{
    [JsonPropertyName("kind")]
    public string? Kind { get; set; }

    [JsonPropertyName("etag")]
    public string? Etag { get; set; }

    [JsonPropertyName("nextSyncToken")]
    public string? NextSyncToken { get; set; }

    [JsonPropertyName("items")]
    public List<CalendarDetailsDto>? Items { get; set; }
}

