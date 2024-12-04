using AllupMVC.Areas.Admin.ViewModels;
using AllupMVC.Areas.Admin.ViewModels;
using AllupMVC.DAL;
using AllupMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupMVC.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
           _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var productsVM=await _context.Products
                .Include(p=>p.Category)
                .Include(p=>p.Brand)
                .Include(p=>p.ProductImages)
                .Select( p=> new GetProductVM
                {
                    Name = p.Name,
                    Id = p.Id,
                    Price = p.Price,
                    CategoryName=p.Category.Name,
                    BrandName=p.Brand.Name,
                    Image=p.ProductImages.FirstOrDefault(p=>p.IsPrimary!=null).Image   
                }
                ).ToListAsync();

           return View(productsVM);  
        }


        public async Task<IActionResult> Create()
        {

            CreateProductVM productVM = new CreateProductVM
            {
                Categories = await _context.Categories.ToListAsync(),
                Brands = await _context.Brands.ToListAsync()

            };
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            productVM.Categories = await _context.Categories.ToListAsync();
            productVM.Brands= await _context.Brands.ToListAsync();
            if(!ModelState.IsValid)
            {
                return View(productVM);
            }




            return View(productVM);
        }

    }
}
