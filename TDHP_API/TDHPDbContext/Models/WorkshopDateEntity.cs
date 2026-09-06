using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TDHP_API.TDHPDbContext.Models
{
    public class WorkshopDateEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid WorkshopId { get; set; }

        [ForeignKey(nameof(WorkshopId))]
        public WorkshopEntity? Workshop { get; set; }

        [Required]
        public string DateText { get; set; } = string.Empty;

        public int? Price { get; set; }
    }
}
