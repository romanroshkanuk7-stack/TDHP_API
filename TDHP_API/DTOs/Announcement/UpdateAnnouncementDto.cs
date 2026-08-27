namespace TDHP_API.DTOs.Announcement
{
    public class UpdateAnnouncementDto
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Link { get; set; }
        public string? LinkLabel { get; set; }
        public bool? IsActive { get; set; }
        public int? SortIndex { get; set; }
    }
}
