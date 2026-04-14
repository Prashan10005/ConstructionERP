using Microsoft.AspNetCore.Mvc;
using ConstructionERP.Services;

namespace ConstructionERP.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISessionService _sessionService;

        public AdminController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public IActionResult Dashboard()
        {
            // active user check
            if(!_sessionService.IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            // admin role check
            if(_sessionService.GetUserRole() != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ViewBag.UserName = _sessionService.GetUsername();
            return View();
        }
    }
}
