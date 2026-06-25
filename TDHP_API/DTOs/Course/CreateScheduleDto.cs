using System.ComponentModel.DataAnnotations;

namespace TDHP_API.DTOs.Course
{
    public class CreateScheduleDto
    {
        [Required]
        public string GroupId { get; set; } = string.Empty;

        [Required]
        public string Time { get; set; } = string.Empty;
    }
}
