using ConstructionERP.Data;
using ConstructionERP.DTOs.Auth;
using ConstructionERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ConstructionERP.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwt;

        public AuthService(ApplicationDbContext context, IJwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email && x.IsActive);

            if (user == null)
                return null;

            var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isValid)
                return null;

            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = _jwt.GenerateToken(
                user.UserID,
                user.Username,
                user.Email,
                user.Role
            );

            return new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                RedirectUrl = user.Role switch
                {
                    "Admin" => "/Admin/Dashboard",
                    "ProjectManager" => "/ProjectManager/Dashboard",
                    "FieldStaff" => "/FieldStaff/Dashboard",
                    _ => "/Auth/Login"
                }
            };
        }
    }
}