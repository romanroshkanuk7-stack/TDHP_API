using System.ComponentModel.DataAnnotations;

namespace TDHP_API.TDHPDbContext.Models
{
    public class WorkshopEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Image { get; set; } = string.Empty;

        [Required]
        public string ToId { get; set; } = string.Empty;

        [Required]
        public bool IsPlaceholder { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfCreate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public ICollection<CustomerEntity> Customers { get; set; } = new List<CustomerEntity>();

    }
}