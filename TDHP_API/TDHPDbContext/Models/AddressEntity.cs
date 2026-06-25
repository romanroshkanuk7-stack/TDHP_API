using System.ComponentModel.DataAnnotations;

namespace TDHP_API.TDHPDbContext.Models
{
    public class AddressEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Street { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public int PostalCode { get; set; }

        public string CustomerId { get; set; } = string.Empty;
        public CustomerEntity? Customer { get; set; }
    }
}
