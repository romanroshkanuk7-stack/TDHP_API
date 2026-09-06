namespace TDHP_API.DTOs.Workshop
{
    public class WorkshopDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string ToId { get; set; } = string.Empty;
        public bool IsPlaceholder { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? Price { get; set; }
        public List<string> Dates { get; set; } = new();
        public List<WorkshopDateItemDto> DateItems { get; set; } = new();
    }
}
