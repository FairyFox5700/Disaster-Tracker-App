using System.Text.Json.Serialization;

namespace DisasterTrackerApp.Models.Calendar;
    public class CreatorDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("self")]
        public bool Self { get; set; }
    }

    public class OrganizerDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("self")]
        public bool Self { get; set; }
    }

    public class StartDto
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }
    }

    public class EndDto
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }
    }

    public class PreferencesDto
    {
        [JsonPropertyName("goo.contactsGivenName")]
        public string GooContactsGivenName { get; set; }

        [JsonPropertyName("goo.contactsEventType")]
        public string GooContactsEventType { get; set; }

        [JsonPropertyName("goo.contactsEmail")]
        public string GooContactsEmail { get; set; }

        [JsonPropertyName("goo.contactsProfileId")]
        public string GooContactsProfileId { get; set; }

        [JsonPropertyName("goo.contactsFullName")]
        public string GooContactsFullName { get; set; }

        [JsonPropertyName("goo.isGPlusUser")]
        public string GooIsGPlusUser { get; set; }

        [JsonPropertyName("goo.contactsIsMyContact")]
        public string GooContactsIsMyContact { get; set; }

        [JsonPropertyName("goo.contactsPhotoUrl")]
        public string GooContactsPhotoUrl { get; set; }
    }

    public class GadgetDto
    {
        [JsonPropertyName("iconLink")]
        public string IconLink { get; set; }

        [JsonPropertyName("preferences")]
        public PreferencesDto PreferencesDto { get; set; }
    }

    public record EventDetailsDto
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("etag")]
        public string Etag { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("htmlLink")]
        public string HtmlLink { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("creator")]
        public CreatorDto CreatorDto { get; set; }

        [JsonPropertyName("organizer")]
        public OrganizerDto OrganizerDto { get; set; }

        [JsonPropertyName("start")]
        public StartDto StartDto { get; set; }

        [JsonPropertyName("end")]
        public EndDto EndDto { get; set; }

        [JsonPropertyName("visibility")]
        public string Visibility { get; set; }

        [JsonPropertyName("iCalUID")]
        public string ICalUID { get; set; }

        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        [JsonPropertyName("gadget")]
        public GadgetDto GadgetDto { get; set; }

        [JsonPropertyName("eventType")]
        public string EventType { get; set; }
    }

    public record GoogleEventsListDto
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("etag")]
        public string Etag { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("accessRole")]
        public string AccessRole { get; set; }

        [JsonPropertyName("defaultReminders")]
        public List<object> DefaultReminders { get; set; }

        [JsonPropertyName("nextSyncToken")]
        public string NextSyncToken { get; set; }

        [JsonPropertyName("items")]
        public List<EventDetailsDto> Items { get; set; }
    }
