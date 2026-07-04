using CbsHaritalandirmaApi.Dtos;

namespace CbsHaritalandirmaApi.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        Task<List<UserResponseDto>> GetUsersAsync();
    }
}
