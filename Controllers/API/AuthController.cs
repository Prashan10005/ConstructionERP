using Microsoft.AspNetCore.Mvc;
using ConstructionERP.DTOs.Auth;
using ConstructionERP.Services.Interfaces;

namespace ConstructionERP.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _auth.LoginAsync(request);

            if (result == null)
                return Unauthorized(new { success = false, message = "Invalid credentials" });

            return Ok(new
            {
                success = true,
                data = result
            });
        }
    }
}
