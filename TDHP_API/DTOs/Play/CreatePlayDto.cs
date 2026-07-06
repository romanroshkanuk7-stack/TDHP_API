using System.ComponentModel.DataAnnotations;

namespace TDHP_API.DTOs.Play
{
    public class CreatePlayDto
    {
        [Required]
        public Guid PerformanceCategoryId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Image { get; set; }

        public string? Description { get; set; }

        public string? CreditsJson { get; set; }

        public string? Target { get; set; }

        public string? Duration { get; set; }

        public int? SortIndex { get; set; }
    }
}
