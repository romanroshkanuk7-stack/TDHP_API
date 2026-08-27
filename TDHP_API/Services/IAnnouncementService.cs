using TDHP_API.DTOs.Announcement;

namespace TDHP_API.Services
{
    public interface IAnnouncementService
    {
        Task<List<AnnouncementDto>> GetActiveAsync();
        Task<List<AnnouncementDto>> GetAllAsync();
        Task<AnnouncementDto?> GetByIdAsync(Guid id);
        Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto);
        Task<AnnouncementDto?> UpdateAsync(Guid id, UpdateAnnouncementDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ReorderAsync(List<Guid> announcementIds);
        Task SeedDefaultAnnouncementsAsync();
    }
}
