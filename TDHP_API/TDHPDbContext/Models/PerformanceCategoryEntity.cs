using System.ComponentModel.DataAnnotations;

namespace TDHP_API.TDHPDbContext.Models
{
    public class PerformanceCategoryEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Image { get; set; } = string.Empty;

        [Required]
        public int SortIndex { get; set; } = 0;

        [Required]
        public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        public ICollection<PlayEntity> Plays { get; set; } = new List<PlayEntity>();
    }
}
