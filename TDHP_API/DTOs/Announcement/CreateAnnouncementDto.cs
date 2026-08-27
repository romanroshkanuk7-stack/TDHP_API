using System.ComponentModel.DataAnnotations;

namespace TDHP_API.DTOs.Announcement
{
    public class CreateAnnouncementDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        public string? Link { get; set; }

        public string? LinkLabel { get; set; }

        public bool? IsActive { get; set; }

        public int? SortIndex { get; set; }
    }
}
