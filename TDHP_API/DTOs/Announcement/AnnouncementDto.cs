namespace TDHP_API.DTOs.Announcement
{
    public class AnnouncementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string? Link { get; set; }
        public string? LinkLabel { get; set; }
        public bool IsActive { get; set; }
        public int SortIndex { get; set; }
        public DateTime DateOfCreate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
