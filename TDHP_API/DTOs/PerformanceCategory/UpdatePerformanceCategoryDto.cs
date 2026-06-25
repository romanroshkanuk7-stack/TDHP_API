namespace TDHP_API.DTOs.PerformanceCategory
{
    public class UpdatePerformanceCategoryDto
    {
        public string? Slug { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public int? SortIndex { get; set; }
    }
}
