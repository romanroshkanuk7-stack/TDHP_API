using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TDHP_API.TDHPDbContext.Models
{
    public class CourseScheduleEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string GroupId { get; set; } = string.Empty;
        public GroupEntity? Group { get; set; }

        [Required]
        public string Time { get; set; } = string.Empty;

        [Required]
        public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public CourseEntity Course { get; set; } = null!;
    }
}