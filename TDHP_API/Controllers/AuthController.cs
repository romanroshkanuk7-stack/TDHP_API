using Microsoft.AspNetCore.Mvc;
using TDHP_API.DTOs.Auth;
using TDHP_API.Services;

namespace TDHP_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _auth.RegisterAsync(dto);
            if (result == null) return BadRequest(new { message = "Registration failed. Email may already be in use." });
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _auth.LoginAsync(dto);
            if (result == null) return Unauthorized(new { message = "Invalid email or password." });
            return Ok(result);
        }
    }
}
