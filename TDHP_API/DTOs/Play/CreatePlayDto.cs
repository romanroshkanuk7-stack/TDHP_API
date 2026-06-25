using System.ComponentModel.DataAnnotations;

namespace TDHP_API.DTOs.Play
{
    public class CreatePlayDto
    {
        [Required]
        public Guid PerformanceCategoryId { get; set; }

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

        public int? SortIndex { get; set; }
    }
}
