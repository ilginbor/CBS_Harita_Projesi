using CbsHaritalandirmaApi.Dtos;
using CbsHaritalandirmaApi.Models;
using CbsHaritalandirmaApi.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CbsHaritalandirmaApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Bu e-posta adresi zaten kayıtlı."
                };
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash,
                FullName = dto.FullName,
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                LastLoginAt = null,
                IsActive = true,
                IsDelete = false
            };

            var createdUser = await _userRepository.AddAsync(user);

            var token = GenerateJwtToken(createdUser);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Kayıt başarılı.",
                Token = token,
                User = MapToUserResponseDto(createdUser)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null || user.IsDelete || !user.IsActive)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "E-posta veya şifre hatalı."
                };
            }

            var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!passwordValid)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "E-posta veya şifre hatalı."
                };
            }

            user.CreatedAt = ToUtc(user.CreatedAt);
            user.LastLoginAt = DateTime.UtcNow;
            user.ModifiedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Giriş başarılı.",
                Token = token,
                User = MapToUserResponseDto(user)
            };
        }

        public async Task<List<UserResponseDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(MapToUserResponseDto).ToList();
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new Exception("JWT Key appsettings.json içinde bulunamadı.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("role", user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserResponseDto MapToUserResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                CreatedAt = ToUtc(user.CreatedAt),
                ModifiedAt = user.ModifiedAt.HasValue ? ToUtc(user.ModifiedAt.Value) : null,
                LastLoginAt = user.LastLoginAt.HasValue ? ToUtc(user.LastLoginAt.Value) : null,
                IsActive = user.IsActive,
                IsDelete = user.IsDelete
            };
        }

        private DateTime ToUtc(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime;
            }

            if (dateTime.Kind == DateTimeKind.Local)
            {
                return dateTime.ToUniversalTime();
            }

            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    }
}