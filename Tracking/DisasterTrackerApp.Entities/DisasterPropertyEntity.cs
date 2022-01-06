using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisasterTrackerApp.Entities
{
    public class DisasterPropertyEntity:IKeyEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public DateTime? Closed { get; set; }
        public ICollection<SourceEntity?>? Sources { get; set; }
        public ICollection<CategoryEntity?>? Categories { get; set; }
    }
}
