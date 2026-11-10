using ActiveClub.Shared.DTOs.Auth;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAsync(RegisterDto dto);
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
    }
}