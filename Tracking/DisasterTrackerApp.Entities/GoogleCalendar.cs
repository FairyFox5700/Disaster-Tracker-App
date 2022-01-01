using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisasterTrackerApp.Entities;

public class GoogleCalendar:IKeyEntity<Guid>,IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string GoogleCalendarId { get; set; }= null!;
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public bool? Primary { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    #region FK
    public Guid UserId { get; set; }
    public GoogleUser User { get; set; }= null!;
    public List<CalendarEvent> CalendarEvents { get; set; } = default!;

    #endregion
}