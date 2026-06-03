using Microsoft.AspNetCore.Mvc;

namespace VulnerableApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
