using System.ComponentModel.DataAnnotations;

namespace TDHP_API.TDHPDbContext.Models
{
    public class CustomerEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string SecondName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        // string — щоб підтримувати формат +420..., провідні нулі
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime Birthday { get; set; }

        public string AddressId { get; set; } = string.Empty;
        public AddressEntity? Address { get; set; }

        public bool Paid { get; set; } = false;

        public ICollection<CourseEntity> Courses { get; set; } = new List<CourseEntity>();

        public Guid? WorkshopId { get; set; }
        public WorkshopEntity? Workshop { get; set; }

        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;

        public string? Category { get; set; }
        public string? LessonsFrequency { get; set; }
        public int? Price { get; set; }
    }
}
