using ConstructionERP.Data;
using ConstructionERP.DTOs.User;
using ConstructionERP.Models.Entities;
using ConstructionERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConstructionERP.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            // Check duplicate username
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (existingUser != null)
                throw new Exception("Username already exists");

            // Check duplicate email
            var existingEmail = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingEmail != null)
                throw new Exception("Email already exists");

            // Create user
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => u.Role != "Admin")
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new UserDto
                {
                    UserID = u.UserID,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt
                })
                .ToListAsync();
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            // duplicate username check
            var usernameExists = await _context.Users
                .AnyAsync(x => x.Username == dto.Username && x.UserID != id);

            if (usernameExists)
                throw new Exception("Username already exists");

            // duplicate email check
            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == dto.Email && x.UserID != id);

            if (emailExists)
                throw new Exception("Email already exists");

            // Update basic fields
            user.Username = dto.Username;
            user.Email = dto.Email;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            // only update password if provided
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // controlling data exposed to client by mapping to DTO
        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
    }
}