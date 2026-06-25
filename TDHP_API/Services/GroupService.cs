using Microsoft.EntityFrameworkCore;
using TDHP_API.DTOs.Group;
using TDHP_API.TDHPDbContext;
using TDHP_API.TDHPDbContext.Models;

namespace TDHP_API.Services
{
    public class GroupService : IGroupService
    {
        private readonly THDPContext _db;
        public GroupService(THDPContext db) => _db = db;

        public async Task<List<GroupDto>> GetAllAsync() =>
            await _db.Groups.Select(g => new GroupDto { Id = g.Id, Name = g.Name }).ToListAsync();

        public async Task<GroupDto?> GetByIdAsync(string id) =>
            await _db.Groups.Where(g => g.Id == id)
                .Select(g => new GroupDto { Id = g.Id, Name = g.Name }).FirstOrDefaultAsync();

        public async Task<GroupDto> CreateAsync(CreateGroupDto dto)
        {
            var entity = new GroupEntity { Name = dto.Name };
            _db.Groups.Add(entity);
            await _db.SaveChangesAsync();
            return new GroupDto { Id = entity.Id, Name = entity.Name };
        }

        public async Task<GroupDto?> UpdateAsync(string id, CreateGroupDto dto)
        {
            var entity = await _db.Groups.FindAsync(id);
            if (entity == null) return null;
            entity.Name = dto.Name;
            await _db.SaveChangesAsync();
            return new GroupDto { Id = entity.Id, Name = entity.Name };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.Groups.FindAsync(id);
            if (entity == null) return false;
            _db.Groups.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
