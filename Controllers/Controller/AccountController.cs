using Microsoft.AspNetCore.Mvc;
using ConstructionERP.Services;

namespace ConstructionERP.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
