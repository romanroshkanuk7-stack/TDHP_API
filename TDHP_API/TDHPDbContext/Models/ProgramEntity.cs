using System.ComponentModel.DataAnnotations;

namespace TDHP_API.TDHPDbContext.Models
{
    public class ProgramEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string DateLine1 { get; set; } = string.Empty;

        [Required]
        public string DateLine2 { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Image { get; set; } = string.Empty;

        public string? Time { get; set; }

        public string? Location { get; set; }

        public string? ButtonLink { get; set; }

        [Required]
        public int SortIndex { get; set; } = 0;

        [Required]
        public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
