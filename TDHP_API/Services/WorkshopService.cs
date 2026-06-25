using Microsoft.EntityFrameworkCore;
using TDHP_API.DTOs.Workshop;
using TDHP_API.TDHPDbContext;
using TDHP_API.TDHPDbContext.Models;

namespace TDHP_API.Services
{
    public class WorkshopService : IWorkshopService
    {
        private readonly THDPContext _db;
        public WorkshopService(THDPContext db) => _db = db;

        public async Task<List<WorkshopDto>> GetAllAsync() =>
            await _db.Workshops.Select(w => ToDto(w)).ToListAsync();

        public async Task<WorkshopDto?> GetByIdAsync(Guid id) =>
            await _db.Workshops.Where(w => w.Id == id).Select(w => ToDto(w)).FirstOrDefaultAsync();

        public async Task<WorkshopDto> CreateAsync(CreateWorkshopDto dto)
        {
            var entity = new WorkshopEntity { Title = dto.Title, Image = dto.Image, ToId = dto.ToId, IsPlaceholder = dto.IsPlaceholder, Description = dto.Description };
            _db.Workshops.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<WorkshopDto?> UpdateAsync(Guid id, UpdateWorkshopDto dto)
        {
            var entity = await _db.Workshops.FindAsync(id);
            if (entity == null) return null;
            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.Image != null) entity.Image = dto.Image;
            if (dto.ToId != null) entity.ToId = dto.ToId;
            if (dto.IsPlaceholder.HasValue) entity.IsPlaceholder = dto.IsPlaceholder.Value;
            if (dto.Description != null) entity.Description = dto.Description;
            entity.LastUpdate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Workshops.FindAsync(id);
            if (entity == null) return false;
            _db.Workshops.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        private static WorkshopDto ToDto(WorkshopEntity w) => new() { Id = w.Id, Title = w.Title, Image = w.Image, ToId = w.ToId, IsPlaceholder = w.IsPlaceholder, Description = w.Description };

        public async Task SeedDefaultWorkshopsAsync()
        {
            if (await _db.Workshops.AnyAsync()) return;

            var defaultDesc = "Jedná se o fyzickou aktivitu, která zahrnuje náročné a koordinované pohyby těla, jako jsou skoky, salta, stojky, výdrže nebo rovnovážné prvky. Vyžaduje sílu, pružnost, rovnováhu, koordinaci, obratnost a odvahu. V našich pravidelných lekcích se zaměřujeme převážně na nízkou taneční akrobacii, kterou využívají tanečníci, ale okrajově se dotýkáme také párové akrobacie a v rámci víkendových workshopů i závěsné akrobacie na šálách.";

            var items = new List<WorkshopEntity>
            {
                new() { Title = "VÍKENDOVÉ WORKSHOPY", Image = "/06_barva.jpg", ToId = "VÍKENDOVÉ", IsPlaceholder = false, Description = defaultDesc },
                new() { Title = "LETNÍ ŠKOLA", Image = "/20260109-0019.jpg", ToId = "LETNÍŠKOLA", IsPlaceholder = false, Description = defaultDesc },
                new() { Title = "OPEN CLASSES", Image = "/06_barva.jpg", ToId = "OPENCLASSES", IsPlaceholder = false, Description = defaultDesc },
                new() { Title = "HRÁTKY", Image = "/tanecni_divadlo.jpg", ToId = "HRÁTKY", IsPlaceholder = false, Description = defaultDesc }
            };

            _db.Workshops.AddRange(items);
            await _db.SaveChangesAsync();
        }
    }
}
