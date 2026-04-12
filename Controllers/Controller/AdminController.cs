using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ConstructionERP.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            // active user check
            if(HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // admin role check
            if(HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("Username");
            ViewBag.UserEnail = HttpContext.Session.GetString("UserEmail");

            return View();
        }
    }
}
