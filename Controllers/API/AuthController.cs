using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public readonly ISessionService _sessionService; // dependacy

        public AuthController(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if(!ModelState.IsValid)
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
                    return Unauthorized(new { success = false, message = "Invalid email or password" });
                }

                // verify password
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

                if (!isValidPassword)
                {
                    return Unauthorized(new { success = false, message = "Invalid email or password" });
                }

                user.LastLoginAt = DateTime.Now;
                await _context.SaveChangesAsync();

                _sessionService.SetUserSession(user.UserID, user.Username, user.Email, user.Role);

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
                    sucess = true,
                    message = "Logoin Sccessful",
                    userId = user.UserID,
                    username = user.Username,
                    email = user.Email,
                    role = user.Role,
                    redirectUrl = redirect,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred :" + ex.Message });
            }
        }

        //Creating user API
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel model)
        {
            // check whether current user is Admin
            var currentUserRole = _sessionService.GetUserRole();
            if (currentUserRole != "Admin")
            {
                return Unauthorized(new { success = false, message = "Only Admin can create users" });
            }

            if(!ModelState.IsValid)
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
        public async Task<IActionResult> GetUser()
        {
            // check for admin role 
            var currentUserRole = _sessionService.GetUserRole();
            if(currentUserRole != "Admin")
            {
                return Unauthorized(new { success = false, message = "You are not Authorized" });
            }

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

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            Response.Cookies.Delete(".AspNetCore.Session"); // wont allow to resuse the same session

            return Ok(new { success = true, message = "Logged out successfully" });
        }
    }
}
