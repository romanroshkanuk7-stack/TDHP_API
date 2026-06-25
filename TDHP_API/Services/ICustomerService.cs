using TDHP_API.DTOs.Customer;
namespace TDHP_API.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(string id);
        Task<List<CustomerDto>> GetByCourseAsync(Guid courseId);
        Task<List<CustomerDto>> GetByWorkshopAsync(Guid workshopId);
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
        Task<CustomerDto?> UpdateAsync(string id, UpdateCustomerDto dto);
        Task<bool> SetPaidAsync(string id, bool paid);
        Task<bool> DeleteAsync(string id);
    }
}
