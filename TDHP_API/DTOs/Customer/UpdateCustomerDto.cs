namespace TDHP_API.DTOs.Customer
{
    public class UpdateCustomerDto
    {
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public Guid? CourseId { get; set; }
        public List<Guid>? CourseIds { get; set; }
        public Guid? WorkshopId { get; set; }
        public string? Category { get; set; }
        public string? LessonsFrequency { get; set; }
        public int? Price { get; set; }
    }
}
