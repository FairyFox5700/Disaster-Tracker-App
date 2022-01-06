using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;

namespace DisasterTrackerApp.Entities;

public class DisasterEvent:IKeyEntity<Guid>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public DisasterPropertyEntity Properties { get; set; }
    [JsonConverter(typeof(GeometryConverter))]
    public Geometry Geometry { get; set; }
}