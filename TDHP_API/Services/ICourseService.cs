using TDHP_API.DTOs.Course;
namespace TDHP_API.Services
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(Guid id);
        Task<CourseDto> CreateAsync(CreateCourseDto dto);
        Task<CourseDto?> UpdateAsync(Guid id, UpdateCourseDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ReorderAsync(List<Guid> courseIds);
        Task<ScheduleDto?> AddScheduleAsync(string courseIdOrKey, CreateScheduleDto dto);
        Task<ScheduleDto?> UpdateScheduleAsync(string courseIdOrKey, Guid scheduleId, CreateScheduleDto dto);
        Task<bool> DeleteScheduleAsync(string courseIdOrKey, Guid scheduleId);
    }
}
