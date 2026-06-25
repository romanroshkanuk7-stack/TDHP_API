namespace TDHP_API.DTOs.Play
{
    public class UpdatePlayDto
    {
        public Guid? PerformanceCategoryId { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? CreditsJson { get; set; }
        public string? Target { get; set; }
        public string? Duration { get; set; }
        public int? SortIndex { get; set; }
    }
}
