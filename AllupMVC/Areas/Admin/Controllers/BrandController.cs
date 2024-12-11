using AllupMVC.Areas.Admin.ViewModels;
using AllupMVC.DAL;
using AllupMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AllupMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;


        public BrandController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var brandsVM = await _context.Brands.Where(c => !c.IsDeleted).Include(c => c.Products).Select(c => new GetBrandVM

            {
                Id = c.Id,
                Name = c.Name,
                ProductCount = c.Products.Count

            }

             ).ToListAsync();


            return View(brandsVM);
        }

        public IActionResult Create()
        {
            CreateBrandVM brandVM = new CreateBrandVM();
            return View(brandVM);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandVM brandVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            bool result = await _context.Brands.AnyAsync(c => c.Name.Trim() == brandVM.Name.Trim());
            if (result)
            {
                ModelState.AddModelError("Name", "Brand already exists");
                return View(brandVM);
            }

            Brand brand = new()
            {
                Name = brandVM.Name
            };


            brand.CreatedAt = DateTime.Now;
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);

            if (brand == null) return NotFound();
            UpdateBrandVM brandVM = new()
            {
                Name = brand.Name,
                Id = brand.Id
            };

            return View(brandVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateBrandVM brandVM)
        {
            if (id == null || id < 1) return BadRequest();

            Brand existed = await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);
            if (brandVM == null) return NotFound();


            if (!ModelState.IsValid)
            {
                return View(brandVM);
            }
            bool result = await _context.Brands.AnyAsync(c => c.Name.Trim() == brandVM.Name.Trim() && c.Id != id);
            if (result)
            {
                ModelState.AddModelError(nameof(brandVM.Name), "Brand already exists");
                return View(brandVM);

            }

            existed.Name = brandVM.Name;
            _context.Brands.Update(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Brand brand = await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);
            if (brand == null) return NotFound();
            brand.IsDeleted = true;
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

