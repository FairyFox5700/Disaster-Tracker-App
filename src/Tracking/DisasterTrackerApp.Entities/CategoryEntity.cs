using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisasterTrackerApp.Entities
{
    public class CategoryEntity : IKeyEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? ExternalApiId { get; set; }
        public string? Title { get; set; }
    }
}
