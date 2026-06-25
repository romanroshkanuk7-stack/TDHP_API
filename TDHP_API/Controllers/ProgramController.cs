using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDHP_API.DTOs.Program;
using TDHP_API.Services;

namespace TDHP_API.Controllers
{
    [ApiController]
    [Route("api/programs")]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramService _service;
        public ProgramController(IProgramService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var program = await _service.GetByIdAsync(id);
            return program == null ? NotFound() : Ok(program);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] CreateProgramDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}"), Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProgramDto dto)
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
        public async Task<IActionResult> Reorder([FromBody] List<Guid> programIds)
        {
            var success = await _service.ReorderAsync(programIds);
            return success ? NoContent() : BadRequest(new { message = "Failed to reorder program items." });
        }
    }
}
