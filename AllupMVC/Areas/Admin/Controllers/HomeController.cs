using AllupMVC.DAL;
using Microsoft.AspNetCore.Mvc;

namespace AllupMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
           _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
