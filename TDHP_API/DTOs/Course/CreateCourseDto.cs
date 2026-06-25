using System.ComponentModel.DataAnnotations;
namespace TDHP_API.DTOs.Course
{
    public class CreateCourseDto
    {
        [Required] public string CourseKey { get; set; } = string.Empty;
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public string Image { get; set; } = string.Empty;
        [Required] public string DetailImage { get; set; } = string.Empty;
        [Required] public string Description { get; set; } = string.Empty;
        [Required] public string ButtonText { get; set; } = string.Empty;
        public string? VideoLink { get; set; }
        public int? SortIndex { get; set; }
    }
}
