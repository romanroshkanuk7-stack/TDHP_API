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
        private readonly ILogger<WorkshopController> _logger;

        public WorkshopController(IWorkshopService service, ILogger<WorkshopController> logger)
        {
            _service = service;
            _logger = logger;
        }

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
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workshop");
                return StatusCode(500, new { message = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id:guid}"), Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWorkshopDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workshop {Id}", id);
                return StatusCode(500, new { message = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        [HttpDelete("{id:guid}"), Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting workshop {Id}", id);
                return StatusCode(500, new { message = ex.Message, inner = ex.InnerException?.Message });
            }
        }
    }
}
