using AllupMVC.DAL;
using AllupMVC.Models;
using AllupMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            Product? product = await _context.Products
            .Include(p => p.ProductImages
            .OrderByDescending(pi => pi.IsPrimary))
            .Include(p=>p.Category)
            .Include(p=>p.Brand)
            .FirstOrDefaultAsync(p=>p.Id==id);


            if(product==null) return NotFound();


            DetailVM detailVM = new DetailVM
            {
                Product = product,
                RelatedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != id)
                .Include(p=>p.ProductImages
                .Where(p=>p.IsPrimary!=null))
                .Take(5)
                .ToListAsync()
            };

            return View(detailVM);
        }
    }
}
