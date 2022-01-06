using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.Entities;
using Google.Apis.Calendar.v3.Data;

namespace DisasterTrackerApp.BL.Helpers;

public class CalendarEventConverter : ICalendarEventConverter
{
    private readonly IGoogleGeocoderService _geocoderService;

    public CalendarEventConverter(IGoogleGeocoderService geocoderService)
    {
        _geocoderService = geocoderService;
    }
    
    public async Task<CalendarEvent> ToCalendarEventEntity(Event googleEvent, Guid calendarId)
    {
        var coordinates = string.IsNullOrWhiteSpace(googleEvent.Location)
            ? null
            : await _geocoderService.GetLocationCoordinates(googleEvent.Location);
        
        return new CalendarEvent
        {
            GoogleEventId = googleEvent.Id,
            CalendarId = calendarId,
            Location = googleEvent.Location,
            StartedTs = GetUtcDateTime(googleEvent.Start),
            EndTs = GetUtcDateTime(googleEvent.End),
            Summary = googleEvent.Summary,
            Coordinates = coordinates
        };
    }
    
    private DateTime? GetUtcDateTime(EventDateTime eventDateTime)
    {
        if (eventDateTime.DateTime != null)
        {
            return eventDateTime.DateTime.Value.ToUniversalTime();
        }

        if (DateTime.TryParse(eventDateTime.Date, out var onlyDate))
        {
            return DateTime.SpecifyKind(onlyDate, DateTimeKind.Utc);
        }

        return null;
    }
}