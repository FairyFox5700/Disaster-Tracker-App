using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Point = NetTopologySuite.Geometries.Point;

namespace DisasterTrackerApp.Entities;

public class DisasterEvent:IKeyEntity<Guid>, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string ExternalApiId { get; set; }= null!;
    public string? Tittle { get; set; }
    public string? CategoryTittle { get; set; }
    public string? Description { get; set; }
    public bool? Active { get; set; }
    public  Point Coordiantes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}