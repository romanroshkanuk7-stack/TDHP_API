namespace TDHP_API.DTOs.Course
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string CourseKey { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string DetailImage { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
        public int SortIndex { get; set; }
        public string VideoLink { get; set; } = string.Empty;
        public List<ScheduleDto> Schedules { get; set; } = new();
    }
    public class ScheduleDto
    {
        public Guid Id { get; set; }
        public string Time { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
    }
}
