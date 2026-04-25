using Microsoft.AspNetCore.Mvc;

namespace ConstructionERP.Controllers
{
    public class ProjectManagerController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Project()
        {
            return View(); 
        }

        public IActionResult Material()
        {
            return View(); 
        }
    }
}
