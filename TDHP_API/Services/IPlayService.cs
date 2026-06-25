using TDHP_API.DTOs.Play;

namespace TDHP_API.Services
{
    public interface IPlayService
    {
        Task<List<PlayDto>> GetAllAsync();
        Task<PlayDto?> GetByIdAsync(Guid id);
        Task<List<PlayDto>> GetByCategoryAsync(Guid categoryId);
        Task<PlayDto> CreateAsync(CreatePlayDto dto);
        Task<PlayDto?> UpdateAsync(Guid id, UpdatePlayDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ReorderAsync(List<Guid> playIds);
        Task SeedDefaultPlaysAsync();
    }
}
