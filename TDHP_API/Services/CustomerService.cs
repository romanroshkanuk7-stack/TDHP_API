using Microsoft.EntityFrameworkCore;
using TDHP_API.DTOs.Customer;
using TDHP_API.TDHPDbContext;
using TDHP_API.TDHPDbContext.Models;

namespace TDHP_API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly THDPContext _db;
        public CustomerService(THDPContext db) => _db = db;

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var list = await _db.Customers
                .Include(c => c.Courses)
                .Include(c => c.Workshop)
                .Include(c => c.Address)
                .ToListAsync();
            return list.Select(c => ToDto(c)).ToList();
        }

        public async Task<CustomerDto?> GetByIdAsync(string id)
        {
            var entity = await _db.Customers
                .Include(c => c.Courses)
                .Include(c => c.Workshop)
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
            return entity == null ? null : ToDto(entity);
        }

        public async Task<List<CustomerDto>> GetByCourseAsync(Guid courseId)
        {
            var list = await _db.Customers
                .Include(c => c.Courses)
                .Include(c => c.Address)
                .Where(c => c.Courses.Any(co => co.Id == courseId))
                .ToListAsync();
            return list.Select(c => ToDto(c)).ToList();
        }

        public async Task<List<CustomerDto>> GetByWorkshopAsync(Guid workshopId)
        {
            var list = await _db.Customers
                .Include(c => c.Workshop)
                .Include(c => c.Address)
                .Where(c => c.WorkshopId == workshopId)
                .ToListAsync();
            return list.Select(c => ToDto(c)).ToList();
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            var entity = new CustomerEntity
            {
                Name = dto.Name, SecondName = dto.SecondName, Email = dto.Email,
                PhoneNumber = dto.PhoneNumber, Birthday = DateTime.SpecifyKind(dto.Birthday, DateTimeKind.Utc),
                WorkshopId = dto.WorkshopId,
                Category = dto.Category, LessonsFrequency = dto.LessonsFrequency, Price = dto.Price
            };

            var courseIdsToQuery = new List<Guid>();
            if (dto.CourseIds != null && dto.CourseIds.Count > 0)
            {
                courseIdsToQuery.AddRange(dto.CourseIds);
            }
            else if (dto.CourseId.HasValue)
            {
                courseIdsToQuery.Add(dto.CourseId.Value);
            }

            if (courseIdsToQuery.Count > 0)
            {
                entity.Courses = await _db.Courses.Where(c => courseIdsToQuery.Contains(c.Id)).ToListAsync();
            }

            if (dto.Street != null)
            {
                entity.Address = new AddressEntity
                {
                    Street = dto.Street, City = dto.City ?? string.Empty,
                    PostalCode = dto.PostalCode ?? 0, CustomerId = entity.Id
                };
            }
            _db.Customers.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<CustomerDto?> UpdateAsync(string id, UpdateCustomerDto dto)
        {
            var entity = await _db.Customers.Include(c => c.Courses).FirstOrDefaultAsync(c => c.Id == id);
            if (entity == null) return null;
            if (dto.Name != null) entity.Name = dto.Name;
            if (dto.SecondName != null) entity.SecondName = dto.SecondName;
            if (dto.Email != null) entity.Email = dto.Email;
            if (dto.PhoneNumber != null) entity.PhoneNumber = dto.PhoneNumber;
            if (dto.Birthday.HasValue) entity.Birthday = DateTime.SpecifyKind(dto.Birthday.Value, DateTimeKind.Utc);
            
            if (dto.CourseIds != null)
            {
                entity.Courses = await _db.Courses.Where(c => dto.CourseIds.Contains(c.Id)).ToListAsync();
            }
            else if (dto.CourseId.HasValue)
            {
                entity.Courses = await _db.Courses.Where(c => c.Id == dto.CourseId.Value).ToListAsync();
            }

            if (dto.WorkshopId.HasValue) entity.WorkshopId = dto.WorkshopId;
            if (dto.Category != null) entity.Category = dto.Category;
            if (dto.LessonsFrequency != null) entity.LessonsFrequency = dto.LessonsFrequency;
            if (dto.Price.HasValue) entity.Price = dto.Price.Value;
            entity.LastUpdate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> SetPaidAsync(string id, bool paid)
        {
            var entity = await _db.Customers.FindAsync(id);
            if (entity == null) return false;
            entity.Paid = paid;
            entity.LastUpdate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.Customers.FindAsync(id);
            if (entity == null) return false;
            _db.Customers.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        private static CustomerDto ToDto(CustomerEntity c) => new()
        {
            Id = c.Id, Name = c.Name, SecondName = c.SecondName,
            Email = c.Email, PhoneNumber = c.PhoneNumber, Birthday = c.Birthday,
            Paid = c.Paid,
            CourseId = c.Courses?.FirstOrDefault()?.Id,
            CourseName = c.Courses?.FirstOrDefault()?.Title,
            CourseIds = c.Courses?.Select(co => co.Id).ToList() ?? new List<Guid>(),
            CourseNames = c.Courses?.Select(co => co.Title).ToList() ?? new List<string>(),
            WorkshopId = c.WorkshopId, WorkshopName = c.Workshop?.Title,
            Street = c.Address?.Street ?? string.Empty,
            City = c.Address?.City ?? string.Empty,
            PostalCode = c.Address?.PostalCode,
            Category = c.Category,
            LessonsFrequency = c.LessonsFrequency,
            Price = c.Price
        };
    }
}
