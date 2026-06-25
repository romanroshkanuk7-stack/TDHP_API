using System.ComponentModel.DataAnnotations;

namespace TDHP_API.DTOs.PerformanceCategory
{
    public class CreatePerformanceCategoryDto
    {
        [Required]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Image { get; set; } = string.Empty;

        public int? SortIndex { get; set; }
    }
}
