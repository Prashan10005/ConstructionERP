using Microsoft.AspNetCore.Mvc;
using ConstructionERP.Services;

namespace ConstructionERP.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISessionService _sessionService;

        public AccountController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // check for login status
            if (_sessionService.IsUserLoggedIn())
            {
                var role = _sessionService.GetUserRole();
                return role switch
                    {
                    "Admin" => RedirectToAction("Dashboard", "Admin"),
                    "ProjectManager" => RedirectToAction("Dashboard", "ProjectManager"),
                    "FieldStaff" => RedirectToAction("Dashboard", "FieldStaff"),
                    _ => RedirectToAction("Login")
                };
            }  
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
