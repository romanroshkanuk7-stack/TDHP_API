using Microsoft.EntityFrameworkCore;
using TDHP_API.DTOs.Program;
using TDHP_API.TDHPDbContext;
using TDHP_API.TDHPDbContext.Models;

namespace TDHP_API.Services
{
    public class ProgramService : IProgramService
    {
        private readonly THDPContext _db;
        public ProgramService(THDPContext db) => _db = db;

        public async Task<List<ProgramDto>> GetAllAsync() =>
            await _db.Programs
                .OrderBy(p => p.SortIndex)
                .ThenBy(p => p.DateOfCreate)
                .Select(p => ToDto(p))
                .ToListAsync();

        public async Task<ProgramDto?> GetByIdAsync(Guid id) =>
            await _db.Programs
                .Where(p => p.Id == id)
                .Select(p => ToDto(p))
                .FirstOrDefaultAsync();

        public async Task<ProgramDto> CreateAsync(CreateProgramDto dto)
        {
            int sortIndex = dto.SortIndex ?? 0;
            if (!dto.SortIndex.HasValue)
            {
                var maxSortIndex = await _db.Programs.Select(p => (int?)p.SortIndex).MaxAsync() ?? -1;
                sortIndex = maxSortIndex + 1;
            }

            var entity = new ProgramEntity
            {
                DateLine1 = dto.DateLine1,
                DateLine2 = dto.DateLine2,
                Title = dto.Title,
                Image = dto.Image,
                SortIndex = sortIndex
            };
            _db.Programs.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<ProgramDto?> UpdateAsync(Guid id, UpdateProgramDto dto)
        {
            var entity = await _db.Programs.FindAsync(id);
            if (entity == null) return null;
            if (dto.DateLine1 != null) entity.DateLine1 = dto.DateLine1;
            if (dto.DateLine2 != null) entity.DateLine2 = dto.DateLine2;
            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.Image != null) entity.Image = dto.Image;
            if (dto.SortIndex.HasValue) entity.SortIndex = dto.SortIndex.Value;
            entity.LastUpdate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Programs.FindAsync(id);
            if (entity == null) return false;
            _db.Programs.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReorderAsync(List<Guid> programIds)
        {
            if (programIds == null || programIds.Count == 0) return false;

            var items = await _db.Programs.Where(p => programIds.Contains(p.Id)).ToListAsync();
            foreach (var item in items)
            {
                var newIndex = programIds.IndexOf(item.Id);
                if (newIndex >= 0)
                {
                    item.SortIndex = newIndex;
                    item.LastUpdate = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync();
            return true;
        }

        private static ProgramDto ToDto(ProgramEntity p) => new()
        {
            Id = p.Id,
            DateLine1 = p.DateLine1,
            DateLine2 = p.DateLine2,
            Title = p.Title,
            Image = p.Image,
            SortIndex = p.SortIndex
        };
    }
}
