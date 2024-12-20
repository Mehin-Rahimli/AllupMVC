using AllupMVC.DAL;
using AllupMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new()
            {
                Slides = await _context.Slides
                .OrderBy(s => s.Order)
                .Take(5)
                .ToListAsync(),

                NewProducts = await _context.Products
                .OrderByDescending(p=>p.CreatedAt)
                .Take(8)
                .Include(p => p.ProductImages
                .Where(pi => pi.IsPrimary != null))
                .ToListAsync()
            };

            return View(homeVM);
        }
    }
}
