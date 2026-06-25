using System.ComponentModel.DataAnnotations;
namespace TDHP_API.DTOs.Customer
{
    public class CreateCustomerDto
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string SecondName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string PhoneNumber { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public Guid? CourseId { get; set; }
        public List<Guid>? CourseIds { get; set; } = new();
        public Guid? WorkshopId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public int? PostalCode { get; set; }
        public string? Category { get; set; }
        public string? LessonsFrequency { get; set; }
        public int? Price { get; set; }
    }
}
