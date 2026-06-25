using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDHP_API.DTOs.Play;
using TDHP_API.Services;

namespace TDHP_API.Controllers
{
    [ApiController]
    [Route("api/plays")]
    public class PlayController : ControllerBase
    {
        private readonly IPlayService _service;
        public PlayController(IPlayService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var play = await _service.GetByIdAsync(id);
            return play == null ? NotFound() : Ok(play);
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            return Ok(await _service.GetByCategoryAsync(categoryId));
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] CreatePlayDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}"), Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePlayDto dto)
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
        public async Task<IActionResult> Reorder([FromBody] List<Guid> playIds)
        {
            var success = await _service.ReorderAsync(playIds);
            return success ? NoContent() : BadRequest(new { message = "Failed to reorder plays." });
        }
    }
}
