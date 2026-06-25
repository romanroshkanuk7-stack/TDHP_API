using TDHP_API.DTOs.Group;
namespace TDHP_API.Services
{
    public interface IGroupService
    {
        Task<List<GroupDto>> GetAllAsync();
        Task<GroupDto?> GetByIdAsync(string id);
        Task<GroupDto> CreateAsync(CreateGroupDto dto);
        Task<GroupDto?> UpdateAsync(string id, CreateGroupDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
