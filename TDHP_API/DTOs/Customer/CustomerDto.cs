namespace TDHP_API.DTOs.Customer
{
    public class CustomerDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public bool Paid { get; set; }
        public Guid? CourseId { get; set; }
        public string? CourseName { get; set; }
        public List<Guid> CourseIds { get; set; } = new();
        public List<string> CourseNames { get; set; } = new();
        public Guid? WorkshopId { get; set; }
        public string? WorkshopName { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int? PostalCode { get; set; }
        public string? Category { get; set; }
        public string? LessonsFrequency { get; set; }
        public int? Price { get; set; }
    }
}
