using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDHP_API.DTOs.Announcement;
using TDHP_API.Services;

namespace TDHP_API.Controllers
{
    [ApiController]
    [Route("api/announcements")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _service;

        public AnnouncementController(IAnnouncementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetActive() => Ok(await _service.GetActiveAsync());

        [HttpGet("all"), Authorize]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:guid}"), Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}"), Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAnnouncementDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
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
        public async Task<IActionResult> Reorder([FromBody] List<Guid> announcementIds)
        {
            var success = await _service.ReorderAsync(announcementIds);
            return success ? NoContent() : BadRequest(new { message = "Failed to reorder announcements." });
        }
    }
}
