using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDHP_API.DTOs.Customer;
using TDHP_API.Services;

namespace TDHP_API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomerController(ICustomerService service) => _service = service;

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            var c = await _service.GetByIdAsync(id);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpGet("course/{courseId:guid}"), Authorize]
        public async Task<IActionResult> GetByCourse(Guid courseId) =>
            Ok(await _service.GetByCourseAsync(courseId));

        [HttpGet("workshop/{workshopId:guid}"), Authorize]
        public async Task<IActionResult> GetByWorkshop(Guid workshopId) =>
            Ok(await _service.GetByWorkshopAsync(workshopId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCustomerDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpPatch("{id}/paid"), Authorize]
        public async Task<IActionResult> SetPaid(string id, [FromBody] bool paid)
        {
            var result = await _service.SetPaidAsync(id, paid);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
