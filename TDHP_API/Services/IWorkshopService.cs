using TDHP_API.DTOs.Workshop;
namespace TDHP_API.Services
{
    public interface IWorkshopService
    {
        Task<List<WorkshopDto>> GetAllAsync();
        Task<WorkshopDto?> GetByIdAsync(Guid id);
        Task<WorkshopDto> CreateAsync(CreateWorkshopDto dto);
        Task<WorkshopDto?> UpdateAsync(Guid id, UpdateWorkshopDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task SeedDefaultWorkshopsAsync();
    }
}
