namespace TDHP_API.DTOs.Course
{
    public class UpdateCourseDto
    {
        public string? CourseKey { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? DetailImage { get; set; }
        public string? Description { get; set; }
        public string? ButtonText { get; set; }
        public int? SortIndex { get; set; }
    }
}
