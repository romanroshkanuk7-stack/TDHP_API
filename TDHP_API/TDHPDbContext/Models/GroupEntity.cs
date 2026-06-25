namespace TDHP_API.TDHPDbContext.Models
{
    public class GroupEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public ICollection<CourseScheduleEntity> Schedules { get; set; } = new List<CourseScheduleEntity>();
    }
}
