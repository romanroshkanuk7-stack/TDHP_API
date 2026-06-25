using TDHP_API.DTOs.PerformanceCategory;

namespace TDHP_API.Services
{
    public interface IPerformanceCategoryService
    {
        Task<List<PerformanceCategoryDto>> GetAllAsync();
        Task<PerformanceCategoryDto?> GetByIdAsync(Guid id);
        Task<PerformanceCategoryDto?> GetBySlugAsync(string slug);
        Task<PerformanceCategoryDto> CreateAsync(CreatePerformanceCategoryDto dto);
        Task<PerformanceCategoryDto?> UpdateAsync(Guid id, UpdatePerformanceCategoryDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ReorderAsync(List<Guid> categoryIds);
        Task SeedDefaultCategoriesAsync();
    }
}
