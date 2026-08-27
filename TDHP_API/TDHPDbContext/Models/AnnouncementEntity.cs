using System.ComponentModel.DataAnnotations;

namespace TDHP_API.TDHPDbContext.Models
{
    public class AnnouncementEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        public string? Link { get; set; }

        public string? LinkLabel { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public int SortIndex { get; set; } = 0;

        [Required]
        public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
