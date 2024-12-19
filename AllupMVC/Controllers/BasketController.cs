using Microsoft.AspNetCore.Mvc;

namespace AllupMVC.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
