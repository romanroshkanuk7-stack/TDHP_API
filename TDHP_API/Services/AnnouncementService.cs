using Microsoft.EntityFrameworkCore;
using TDHP_API.DTOs.Announcement;
using TDHP_API.TDHPDbContext;
using TDHP_API.TDHPDbContext.Models;

namespace TDHP_API.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly THDPContext _db;

        public AnnouncementService(THDPContext db)
        {
            _db = db;
        }

        public async Task<List<AnnouncementDto>> GetActiveAsync() =>
            await _db.Announcements
                .Where(a => a.IsActive)
                .OrderBy(a => a.SortIndex)
                .ThenBy(a => a.DateOfCreate)
                .Select(a => ToDto(a))
                .ToListAsync();

        public async Task<List<AnnouncementDto>> GetAllAsync() =>
            await _db.Announcements
                .OrderBy(a => a.SortIndex)
                .ThenBy(a => a.DateOfCreate)
                .Select(a => ToDto(a))
                .ToListAsync();

        public async Task<AnnouncementDto?> GetByIdAsync(Guid id)
        {
            var entity = await _db.Announcements.FindAsync(id);
            return entity == null ? null : ToDto(entity);
        }

        public async Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto)
        {
            int sortIndex = dto.SortIndex ?? 0;
            if (!dto.SortIndex.HasValue)
            {
                var maxSortIndex = await _db.Announcements.Select(a => (int?)a.SortIndex).MaxAsync() ?? -1;
                sortIndex = maxSortIndex + 1;
            }

            var entity = new AnnouncementEntity
            {
                Title = dto.Title,
                Text = dto.Text,
                Link = dto.Link,
                LinkLabel = dto.LinkLabel,
                IsActive = dto.IsActive ?? true,
                SortIndex = sortIndex
            };

            _db.Announcements.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<AnnouncementDto?> UpdateAsync(Guid id, UpdateAnnouncementDto dto)
        {
            var entity = await _db.Announcements.FindAsync(id);
            if (entity == null) return null;

            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.Text != null) entity.Text = dto.Text;
            if (dto.Link != null) entity.Link = dto.Link;
            if (dto.LinkLabel != null) entity.LinkLabel = dto.LinkLabel;
            if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;
            if (dto.SortIndex.HasValue) entity.SortIndex = dto.SortIndex.Value;

            entity.LastUpdate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Announcements.FindAsync(id);
            if (entity == null) return false;

            _db.Announcements.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReorderAsync(List<Guid> announcementIds)
        {
            if (announcementIds == null || announcementIds.Count == 0) return false;

            var entities = await _db.Announcements.Where(a => announcementIds.Contains(a.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                var newIndex = announcementIds.IndexOf(entity.Id);
                if (newIndex >= 0)
                {
                    entity.SortIndex = newIndex;
                    entity.LastUpdate = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task SeedDefaultAnnouncementsAsync()
        {
            if (await _db.Announcements.AnyAsync()) return;

            var defaults = new List<AnnouncementEntity>
            {
                new AnnouncementEntity
                {
                    Title = "Zápis do kurzů 2026/27",
                    Text = "Přihlašování na kurzy Contemporary dance, akrobacie a neobaletu je spuštěno. Kapacity se rychle plní.",
                    Link = "/kurzy-a-edukace",
                    LinkLabel = "Přihlásit se →",
                    SortIndex = 0
                },
                new AnnouncementEntity
                {
                    Title = "Víkendové workshopy",
                    Text = "Registrace na nadcházející open classes a letní intenzivní kurzy jsou nyní spuštěny.",
                    Link = "/kurzy-a-edukace/workshopy",
                    LinkLabel = "Více info →",
                    SortIndex = 1
                }
            };

            _db.Announcements.AddRange(defaults);
            await _db.SaveChangesAsync();
        }

        private static AnnouncementDto ToDto(AnnouncementEntity a) => new()
        {
            Id = a.Id,
            Title = a.Title,
            Text = a.Text,
            Link = a.Link,
            LinkLabel = a.LinkLabel,
            IsActive = a.IsActive,
            SortIndex = a.SortIndex,
            DateOfCreate = a.DateOfCreate,
            LastUpdate = a.LastUpdate
        };
    }
}
