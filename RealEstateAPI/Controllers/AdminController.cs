using Microsoft.AspNetCore.Mvc;

namespace RealEstateAPI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
