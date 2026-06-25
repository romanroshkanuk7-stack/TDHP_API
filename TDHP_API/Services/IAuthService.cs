using TDHP_API.DTOs.Auth;
namespace TDHP_API.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> RegisterAsync(RegisterDto dto);
        Task<TokenResponseDto?> LoginAsync(LoginDto dto);
    }
}
