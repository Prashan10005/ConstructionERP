using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConstructionERP.Data;
using ConstructionERP.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ConstructionERP.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context; // establish connection

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // check for login status
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var role = HttpContext.Session.GetString("UserRole");
                if (role == "Admin")
                    return RedirectToAction("Dashboard", "Admin");
                else if (role == "ProjectManager")
                    return RedirectToAction("Dashboard", "ProjectManager");
                else
                    return RedirectToAction("Dashboard", "FieldStaff");
            }  
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // prevent unauthorize attack
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.IsActive);

                if (user != null)
                {
                    bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
                    if (isValidPassword)
                    {
                        user.LastLoginAt = DateTime.Now;
                        await _context.SaveChangesAsync();

                        // set session
                        HttpContext.Session.SetString("UserId", user.UserID.ToString());
                        HttpContext.Session.SetString("Username", user.Username);
                        HttpContext.Session.SetString("UserEmail", user.Email);
                        HttpContext.Session.SetString("UserRole", user.Role);

                        // redirect based on role
                        if (user.Role == "Admin")
                            return RedirectToAction("Dashboard", "Admin");
                        else if (user.Role == "ProjectManager")
                            return RedirectToAction("Dashboard", "ProjectManager");
                        else
                            return RedirectToAction("Dashboard", "FieldStaff");
                    }
                }
                ModelState.AddModelError("", "Invalid email or password.");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Successfully Logged out";
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
