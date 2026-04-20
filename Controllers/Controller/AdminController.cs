using Microsoft.AspNetCore.Mvc;
using ConstructionERP.Services;

namespace ConstructionERP.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
