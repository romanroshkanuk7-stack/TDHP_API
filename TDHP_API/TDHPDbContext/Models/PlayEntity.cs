using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TDHP_API.TDHPDbContext.Models
{
    public class PlayEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PerformanceCategoryId { get; set; }

        [ForeignKey(nameof(PerformanceCategoryId))]
        public PerformanceCategoryEntity? PerformanceCategory { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Image { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string CreditsJson { get; set; } = "{}";

        [Required]
        public string Target { get; set; } = string.Empty;

        [Required]
        public string Duration { get; set; } = string.Empty;

        [Required]
        public int SortIndex { get; set; } = 0;

        [Required]
        public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
