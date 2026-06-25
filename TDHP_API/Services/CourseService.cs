using Microsoft.EntityFrameworkCore;
using TDHP_API.DTOs.Course;
using TDHP_API.TDHPDbContext;
using TDHP_API.TDHPDbContext.Models;

namespace TDHP_API.Services
{
    public class CourseService : ICourseService
    {
        private readonly THDPContext _db;
        public CourseService(THDPContext db) => _db = db;

        public async Task<List<CourseDto>> GetAllAsync() =>
            await _db.Courses
                .Include(c => c.Schedules).ThenInclude(s => s.Group)
                .OrderBy(c => c.SortIndex)
                .ThenBy(c => c.DateOfCreate)
                .Select(c => ToDto(c))
                .ToListAsync();

        public async Task<CourseDto?> GetByIdAsync(Guid id) =>
            await _db.Courses
                .Include(c => c.Schedules).ThenInclude(s => s.Group)
                .Where(c => c.Id == id)
                .Select(c => ToDto(c))
                .FirstOrDefaultAsync();

        public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
        {
            int sortIndex = dto.SortIndex ?? 0;
            if (!dto.SortIndex.HasValue)
            {
                var maxSortIndex = await _db.Courses.Select(c => (int?)c.SortIndex).MaxAsync() ?? -1;
                sortIndex = maxSortIndex + 1;
            }

            var entity = new CourseEntity
            {
                CourseKey = dto.CourseKey,
                Title = dto.Title,
                Image = dto.Image,
                DetailImage = dto.DetailImage,
                Description = dto.Description,
                ButtonText = dto.ButtonText,
                SortIndex = sortIndex
            };
            _db.Courses.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<CourseDto?> UpdateAsync(Guid id, UpdateCourseDto dto)
        {
            var entity = await _db.Courses.FindAsync(id);
            if (entity == null) return null;
            if (dto.CourseKey != null) entity.CourseKey = dto.CourseKey;
            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.Image != null) entity.Image = dto.Image;
            if (dto.DetailImage != null) entity.DetailImage = dto.DetailImage;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.ButtonText != null) entity.ButtonText = dto.ButtonText;
            if (dto.SortIndex.HasValue) entity.SortIndex = dto.SortIndex.Value;
            entity.LastUpdate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Courses.FindAsync(id);
            if (entity == null) return false;
            _db.Courses.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ScheduleDto?> AddScheduleAsync(string courseIdOrKey, CreateScheduleDto dto)
        {
            Guid resolvedCourseId;
            if (Guid.TryParse(courseIdOrKey, out var parsedGuid))
            {
                resolvedCourseId = parsedGuid;
            }
            else
            {
                var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseKey == courseIdOrKey);
                if (course == null) return null;
                resolvedCourseId = course.Id;
            }

            var courseExists = await _db.Courses.AnyAsync(c => c.Id == resolvedCourseId);
            if (!courseExists) return null;

            var group = await _db.Groups.FindAsync(dto.GroupId);
            if (group == null) return null;

            var schedule = new CourseScheduleEntity
            {
                Id = Guid.NewGuid(),
                CourseId = resolvedCourseId,
                GroupId = dto.GroupId,
                Time = dto.Time
            };

            _db.CourseSchedules.Add(schedule);
            await _db.SaveChangesAsync();

            return new ScheduleDto
            {
                Id = schedule.Id,
                Time = schedule.Time,
                GroupId = schedule.GroupId,
                GroupName = group.Name
            };
        }

        public async Task<ScheduleDto?> UpdateScheduleAsync(string courseIdOrKey, Guid scheduleId, CreateScheduleDto dto)
        {
            Guid resolvedCourseId;
            if (Guid.TryParse(courseIdOrKey, out var parsedGuid))
            {
                resolvedCourseId = parsedGuid;
            }
            else
            {
                var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseKey == courseIdOrKey);
                if (course == null) return null;
                resolvedCourseId = course.Id;
            }

            var schedule = await _db.CourseSchedules
                .Include(s => s.Group)
                .FirstOrDefaultAsync(s => s.Id == scheduleId && s.CourseId == resolvedCourseId);

            if (schedule == null) return null;

            var group = await _db.Groups.FindAsync(dto.GroupId);
            if (group == null) return null;

            schedule.GroupId = dto.GroupId;
            schedule.Time = dto.Time;

            await _db.SaveChangesAsync();

            return new ScheduleDto
            {
                Id = schedule.Id,
                Time = schedule.Time,
                GroupId = schedule.GroupId,
                GroupName = group.Name
            };
        }

        public async Task<bool> DeleteScheduleAsync(string courseIdOrKey, Guid scheduleId)
        {
            Guid resolvedCourseId;
            if (Guid.TryParse(courseIdOrKey, out var parsedGuid))
            {
                resolvedCourseId = parsedGuid;
            }
            else
            {
                var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseKey == courseIdOrKey);
                if (course == null) return false;
                resolvedCourseId = course.Id;
            }

            var schedule = await _db.CourseSchedules
                .FirstOrDefaultAsync(s => s.Id == scheduleId && s.CourseId == resolvedCourseId);

            if (schedule == null) return false;

            _db.CourseSchedules.Remove(schedule);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReorderAsync(List<Guid> courseIds)
        {
            if (courseIds == null || courseIds.Count == 0) return false;

            var courses = await _db.Courses.Where(c => courseIds.Contains(c.Id)).ToListAsync();
            foreach (var course in courses)
            {
                var newIndex = courseIds.IndexOf(course.Id);
                if (newIndex >= 0)
                {
                    course.SortIndex = newIndex;
                    course.LastUpdate = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync();
            return true;
        }

        private static CourseDto ToDto(CourseEntity c) => new()
        {
            Id = c.Id,
            CourseKey = c.CourseKey,
            Title = c.Title,
            Image = c.Image,
            DetailImage = c.DetailImage,
            Description = c.Description,
            ButtonText = c.ButtonText,
            SortIndex = c.SortIndex,
            Schedules = c.Schedules?.Select(s => new ScheduleDto
            {
                Id = s.Id,
                Time = s.Time,
                GroupId = s.GroupId,
                GroupName = s.Group?.Name ?? string.Empty
            }).ToList() ?? new()
        };
    }
}
