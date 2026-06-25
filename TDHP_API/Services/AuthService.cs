using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TDHP_API.DTOs.Auth;
using TDHP_API.TDHPDbContext.Models;

namespace TDHP_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<UserEntity> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<TokenResponseDto?> RegisterAsync(RegisterDto dto)
        {
            var user = new UserEntity { Email = dto.Email, UserName = dto.UserName };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return null;
            return GenerateToken(user);
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return null;
            var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!valid) return null;
            return GenerateToken(user);
        }

        private TokenResponseDto GenerateToken(UserEntity user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new TokenResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires,
                UserId = user.Id,
                Email = user.Email!
            };
        }
    }
}
