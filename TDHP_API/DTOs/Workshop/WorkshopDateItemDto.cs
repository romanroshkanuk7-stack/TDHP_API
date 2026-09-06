namespace TDHP_API.DTOs.Workshop
{
    public class WorkshopDateItemDto
    {
        public Guid? Id { get; set; }
        public string DateText { get; set; } = string.Empty;
        public int? Price { get; set; }
    }
}
