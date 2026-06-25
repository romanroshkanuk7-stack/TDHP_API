using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDHP_API.DTOs.PerformanceCategory;
using TDHP_API.Services;

namespace TDHP_API.Controllers
{
    [ApiController]
    [Route("api/performance-categories")]
    public class PerformanceCategoryController : ControllerBase
    {
        private readonly IPerformanceCategoryService _service;
        public PerformanceCategoryController(IPerformanceCategoryService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _service.GetByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var category = await _service.GetBySlugAsync(slug);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] CreatePerformanceCategoryDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}"), Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePerformanceCategoryDto dto)
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
        public async Task<IActionResult> Reorder([FromBody] List<Guid> categoryIds)
        {
            var success = await _service.ReorderAsync(categoryIds);
            return success ? NoContent() : BadRequest(new { message = "Failed to reorder performance categories." });
        }
    }
}
