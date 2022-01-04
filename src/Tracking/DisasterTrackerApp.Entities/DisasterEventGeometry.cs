using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Point = NetTopologySuite.Geometries.Point;
using Microsoft.EntityFrameworkCore;

namespace DisasterTrackerApp.Entities
{
    [Owned]
    public class DisasterEventGeometry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public Point Point { get; set; }
        public Guid EventId { get; set; }
    }
}
