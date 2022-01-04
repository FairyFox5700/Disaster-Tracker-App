using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using Point = NetTopologySuite.Geometries.Point;


namespace DisasterTrackerApp.Entities;

public class CalendarEvent:IKeyEntity<Guid>, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string GoogleEventId { get; set; }= null!;
    public string? Summary { get; set; }
    public DateTime StartedTs { get; set; }
    public DateTime EndTs { get; set; }
    public string? Location { get; set; }
    public Point Coordiantes { get; set; }
    
    #region FK
    public string CalendarId { get; set; }= null!;

    public GoogleCalendar Calendar { get; set; } = null!;
    #endregion

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}