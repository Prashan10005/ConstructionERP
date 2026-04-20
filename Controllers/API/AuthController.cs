using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ConstructionERP.Data;
using ConstructionERP.Models.Entities;
using ConstructionERP.Models.ViewModels;
using ConstructionERP.Services;

namespace ConstructionERP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public readonly IJwtService _jwtService; // dependacy

        public AuthController(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Data",
                    error = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // get user and check user status
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.IsActive);

                if (user == null)
                {
                    return Unauthorized(new { success = false, message = "Invalid email or Check Account Status" });
                }

                // verify password
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

                if (!isValidPassword)
                {
                    return Unauthorized(new { success = false, message = "Invalid password" });
                }

                user.LastLoginAt = DateTime.Now;
                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateToken(user.UserID, user.Username, user.Email, user.Role);

                // redirect based on user role
                string redirect = user.Role switch
                {
                    "Admin" => "/Admin/Dashboard",
                    "ProjectManager" => "/ProjectManager/Dashboard",
                    "FieldStaff" => "/FieldStaff/Dashboard",
                    _ => "/Account/Login"
                };

                // return successful session
                return Ok(new
                {
                    success = true,
                    message = "Login Successful",
                    token = token,
                    userId = user.UserID,
                    username = user.Username,
                    email = user.Email,
                    role = user.Role,
                    redirectUrl = redirect,
                    expiresIn = DateTime.UtcNow.AddMinutes(60)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred :" + ex.Message });
            }
        }
        
        // token validation endpoint
        [HttpGet("validate-token")]
        public IActionResult ValidateToken([FromBody] TokenValidationModel model)
        {
            var principal = _jwtService.ValidateToken(model.Token);
            if (principal == null)
            {
                return Unauthorized(new { success = false, message = "Invalid or expired token" });
            }

            return Ok(new { success = true, message = "Token is valid" });
        }
        
        
        //Creating user API
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel model)
        {
            // Get user info from Token
            var userId = _jwtService.GetUserIdFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            try
            {
                // check of existing username or email
                var exisatingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username || u.Email == model.Email);

                if (exisatingUser != null)
                {
                    return BadRequest(new { success = false, message = "Email or Username already exists" });
                }

                // create user
                var newUser = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Role = model.Role,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    LastLoginAt = null,
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "User created successfully",
                    user = new
                    {
                        newUser.UserID,
                        newUser.Username,
                        newUser.Email,
                        newUser.Role,
                        newUser.IsActive,
                        newUser.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error creating user: " + ex.Message });
            }

        }

        //Get all user to grid
        [HttpGet("get-users")]
        [Authorize(Roles = "Admin")] // role based authorization to access this endpoint
        public async Task<IActionResult> GetUser()
        {

            try
            {
                var users = await _context.Users
                                .Where(u => u.Role != "Admin")  // hide admin from the grid
                                .Select(u => new
                                {
                                    u.UserID,
                                    u.Username,
                                    u.Email,
                                    u.Role,
                                    u.IsActive,
                                    u.CreatedAt,
                                    u.LastLoginAt
                                })
                                .OrderByDescending(u => u.CreatedAt)
                                .ToListAsync();

                return Ok(new { success = true, users = users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error fetching users: " + ex.Message });
            }
        }

        // API method to update user by user ID
        [HttpPut("update-user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Data",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // get user by user id
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found" });
                }

                // email duplication check
                var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.UserID != id);
                if (existingEmail != null)
                {
                    return BadRequest(new { success = false, message = "Email already in use by another user" });
                }

                // username duplication check
                var existingUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username && u.UserID != id);

                if (existingUsername != null)
                {
                    return BadRequest(new { success = false, message = "Username already in use by another user" });
                }

                // update user details
                user.Username = model.Username;
                user.Email = model.Email;
                user.Role = model.Role;
                user.IsActive = model.IsActive;

                // check only if password is provided for update
                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "User updated successfully",
                    user = new
                    {
                        user.UserID,
                        user.Username,
                        user.Email,
                        user.Role,
                        user.IsActive,
                        user.CreatedAt,
                        user.LastLoginAt
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error updating user: " + ex.Message });
            }
        }

        // get user details by user id
        [HttpGet("get-user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(int id)
        {

            try
            {
                var user = await _context.Users
                                .Where(u => u.UserID == id)
                                .Select(u => new
                                {
                                    u.UserID,
                                    u.Username,
                                    u.Email,
                                    u.Role,
                                    u.IsActive,
                                    u.CreatedAt,
                                    u.LastLoginAt
                                })
                                .FirstOrDefaultAsync();
                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found" });
                }
                return Ok(new { success = true, user = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error fetching user: " + ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            //removing the token
            return Ok(new { success = true, message = "Logged out successfully" });
        }
    }
}
