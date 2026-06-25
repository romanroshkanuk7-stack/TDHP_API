using Microsoft.EntityFrameworkCore;
using TDHP_API.DTOs.PerformanceCategory;
using TDHP_API.DTOs.Play;
using TDHP_API.TDHPDbContext;
using TDHP_API.TDHPDbContext.Models;

namespace TDHP_API.Services
{
    public class PerformanceCategoryService : IPerformanceCategoryService
    {
        private readonly THDPContext _db;
        public PerformanceCategoryService(THDPContext db) => _db = db;

        public async Task<List<PerformanceCategoryDto>> GetAllAsync()
        {
            var categories = await _db.PerformanceCategories
                .Include(c => c.Plays)
                .OrderBy(c => c.SortIndex)
                .ToListAsync();

            return categories.Select(c => ToDto(c)).ToList();
        }

        public async Task<PerformanceCategoryDto?> GetByIdAsync(Guid id)
        {
            var category = await _db.PerformanceCategories
                .Include(c => c.Plays)
                .FirstOrDefaultAsync(c => c.Id == id);

            return category == null ? null : ToDto(category);
        }

        public async Task<PerformanceCategoryDto?> GetBySlugAsync(string slug)
        {
            var category = await _db.PerformanceCategories
                .Include(c => c.Plays)
                .FirstOrDefaultAsync(c => c.Slug == slug);

            return category == null ? null : ToDto(category);
        }

        public async Task<PerformanceCategoryDto> CreateAsync(CreatePerformanceCategoryDto dto)
        {
            int sortIndex = dto.SortIndex ?? 0;
            if (!dto.SortIndex.HasValue)
            {
                var maxIndex = await _db.PerformanceCategories.Select(c => (int?)c.SortIndex).MaxAsync() ?? -1;
                sortIndex = maxIndex + 1;
            }

            var entity = new PerformanceCategoryEntity
            {
                Slug = dto.Slug,
                Title = dto.Title,
                Image = dto.Image,
                SortIndex = sortIndex
            };

            _db.PerformanceCategories.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<PerformanceCategoryDto?> UpdateAsync(Guid id, UpdatePerformanceCategoryDto dto)
        {
            var entity = await _db.PerformanceCategories.FindAsync(id);
            if (entity == null) return null;

            if (dto.Slug != null) entity.Slug = dto.Slug;
            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.Image != null) entity.Image = dto.Image;
            if (dto.SortIndex.HasValue) entity.SortIndex = dto.SortIndex.Value;

            entity.LastUpdate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.PerformanceCategories.FindAsync(id);
            if (entity == null) return false;

            _db.PerformanceCategories.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReorderAsync(List<Guid> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0) return false;

            var items = await _db.PerformanceCategories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();
            foreach (var item in items)
            {
                var newIndex = categoryIds.IndexOf(item.Id);
                if (newIndex >= 0)
                {
                    item.SortIndex = newIndex;
                    item.LastUpdate = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task SeedDefaultCategoriesAsync()
        {
            if (await _db.PerformanceCategories.AnyAsync()) return;

            var defaults = new List<CreatePerformanceCategoryDto>
            {
                new() { Slug = "pro-nejmensi", Title = "PRO NEJMENŠÍ", Image = "/image 6.png", SortIndex = 0 },
                new() { Slug = "pro-skoly", Title = "PRO ŠKOLY", Image = "/image 7.png", SortIndex = 1 },
                new() { Slug = "pro-dospele", Title = "PRO DOSPĚLÉ", Image = "/image 8.png", SortIndex = 2 },
                new() { Slug = "rodinne", Title = "RODINNÉ", Image = "/image 9.png", SortIndex = 3 }
            };

            foreach (var d in defaults)
            {
                await CreateAsync(d);
            }
        }

        private static PerformanceCategoryDto ToDto(PerformanceCategoryEntity c) => new()
        {
            Id = c.Id,
            Slug = c.Slug,
            Title = c.Title,
            Image = c.Image,
            SortIndex = c.SortIndex,
            Plays = c.Plays?.OrderBy(p => p.SortIndex).Select(p => new PlayDto
            {
                Id = p.Id,
                PerformanceCategoryId = p.PerformanceCategoryId,
                Title = p.Title,
                Image = p.Image,
                Description = p.Description,
                Credits = string.IsNullOrEmpty(p.CreditsJson)
                    ? new Dictionary<string, string>()
                    : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(p.CreditsJson) ?? new(),
                Target = p.Target,
                Duration = p.Duration,
                SortIndex = p.SortIndex
            }).ToList() ?? new()
        };
    }
}
