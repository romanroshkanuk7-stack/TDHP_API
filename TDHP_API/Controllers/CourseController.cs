using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDHP_API.DTOs.Course;
using TDHP_API.Services;

namespace TDHP_API.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        public CourseController(ICourseService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var course = await _service.GetByIdAsync(id);
            return course == null ? NotFound() : Ok(course);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}"), Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourseDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}"), Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPost("reorder"), Authorize]
        public async Task<IActionResult> Reorder([FromBody] List<Guid> courseIds)
        {
            var success = await _service.ReorderAsync(courseIds);
            return success ? NoContent() : BadRequest(new { message = "Failed to reorder courses." });
        }

        [HttpPost("{courseId}/schedules"), Authorize]
        public async Task<IActionResult> AddSchedule(string courseId, [FromBody] CreateScheduleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.AddScheduleAsync(courseId, dto);
            return result == null ? BadRequest(new { message = "Failed to add schedule. Course or Group may not exist." }) : Ok(result);
        }

        [HttpPut("{courseId}/schedules/{id:guid}"), Authorize]
        public async Task<IActionResult> UpdateSchedule(string courseId, Guid id, [FromBody] CreateScheduleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.UpdateScheduleAsync(courseId, id, dto);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{courseId}/schedules/{id:guid}"), Authorize]
        public async Task<IActionResult> DeleteSchedule(string courseId, Guid id)
        {
            var result = await _service.DeleteScheduleAsync(courseId, id);
            return result ? NoContent() : NotFound();
        }
    }
}
