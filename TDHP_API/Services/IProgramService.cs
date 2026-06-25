using TDHP_API.DTOs.Program;

namespace TDHP_API.Services
{
    public interface IProgramService
    {
        Task<List<ProgramDto>> GetAllAsync();
        Task<ProgramDto?> GetByIdAsync(Guid id);
        Task<ProgramDto> CreateAsync(CreateProgramDto dto);
        Task<ProgramDto?> UpdateAsync(Guid id, UpdateProgramDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ReorderAsync(List<Guid> programIds);
    }
}
