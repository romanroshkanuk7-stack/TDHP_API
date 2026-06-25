namespace TDHP_API.DTOs.Workshop
{
    public class UpdateWorkshopDto
    {
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? ToId { get; set; }
        public bool? IsPlaceholder { get; set; }
        public string? Description { get; set; }
    }
}
