using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace DisasterTrackerApp.Entities;

public class DisasterEvent : IKeyEntity<Guid>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string ExternalApiId { get; set; } = null!;
    public string? Title { get; set; }
    public DateTime? Closed { get; set; }
    [Column(TypeName = "geography")]
    public Geometry Geometry { get; set; }
}