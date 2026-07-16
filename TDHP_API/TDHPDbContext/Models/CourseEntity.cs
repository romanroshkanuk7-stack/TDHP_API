using System.ComponentModel.DataAnnotations;

namespace TDHP_API.TDHPDbContext.Models
{
    public class CourseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string CourseKey { get; set; } = string.Empty; // neobalet

        [Required]
        public string Title { get; set; } = string.Empty; // NEOBALET

        [Required]
        public string Image { get; set; } = string.Empty;

        [Required]
        public string DetailImage { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ButtonText { get; set; } = string.Empty;

        [Required]
        public string VideoLink { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        [Required]
        public int SortIndex { get; set; } = 0;

        [Required]
        public int PriceKids1 { get; set; } = 2700;

        [Required]
        public int PriceKids2 { get; set; } = 4100;

        [Required]
        public int PriceAdults1 { get; set; } = 3500;

        [Required]
        public int PriceAdults2 { get; set; } = 4500;

        public ICollection<CourseScheduleEntity> Schedules { get; set; } = new List<CourseScheduleEntity>();
        public ICollection<CustomerEntity> Customers { get; set; } = new List<CustomerEntity>();
    }
}