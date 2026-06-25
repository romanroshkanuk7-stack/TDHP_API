using TDHP_API.DTOs.Play;

namespace TDHP_API.DTOs.PerformanceCategory
{
    public class PerformanceCategoryDto
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int SortIndex { get; set; }
        public List<PlayDto> Plays { get; set; } = new();
    }
}
