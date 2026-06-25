using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDHP_API.DTOs.Workshop;
using TDHP_API.Services;

namespace TDHP_API.Controllers
{
    [ApiController]
    [Route("api/workshops")]
    public class WorkshopController : ControllerBase
    {
        private readonly IWorkshopService _service;
        public WorkshopController(IWorkshopService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var w = await _service.GetByIdAsync(id);
            return w == null ? NotFound() : Ok(w);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] CreateWorkshopDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}"), Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWorkshopDto dto)
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
    }
}
