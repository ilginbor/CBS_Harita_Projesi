using CbsHaritalandirmaApi.Dtos;
using CbsHaritalandirmaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CbsHaritalandirmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<UserResponseDto>>> GetUsers()
        {
            var users = await _authService.GetUsersAsync();

            return Ok(users);
        }
    }
}
