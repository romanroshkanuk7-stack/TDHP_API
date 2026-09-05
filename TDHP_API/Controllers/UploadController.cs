using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TDHP_API.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UploadController> _logger;

        public UploadController(IWebHostEnvironment env, IConfiguration configuration, ILogger<UploadController> logger)
        {
            _env = env;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Soubor nebyl vybrán nebo je prázdný." });
            }

            // Validate file extension (only allow images)
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = "Neplatný typ souboru. Jsou povoleny pouze obrázky (jpg, png, gif, webp, svg)." });
            }

            try
            {
                // Ensure directory exists
                var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Dynamic URL pointing to backend serving static files
                var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var fileUrl = $"{baseUrl}/uploads/{fileName}";

                return Ok(new { url = fileUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chyba při nahrávání obrázku.");
                return StatusCode(500, new { message = $"Chyba při nahrávání obrázku: {ex.Message}" });
            }
        }
    }
}
