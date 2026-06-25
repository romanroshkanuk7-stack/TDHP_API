namespace TDHP_API.DTOs.Play
{
    public class PlayDto
    {
        public Guid Id { get; set; }
        public Guid PerformanceCategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, string> Credits { get; set; } = new();
        public string Target { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int SortIndex { get; set; }
    }
}
