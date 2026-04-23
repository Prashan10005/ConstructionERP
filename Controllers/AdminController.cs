using Microsoft.AspNetCore.Mvc;

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
