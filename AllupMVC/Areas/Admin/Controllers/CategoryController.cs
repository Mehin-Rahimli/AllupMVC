using AllupMVC.DAL;
using AllupMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Category>categories=await _context.Categories.Where(c=>!c.IsDeleted).Include(c=>c.Products).ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            category.CreatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            
            Category category=await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(category == null) return NotFound();
            return View(category);

        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            if (id == null || id < 1) return BadRequest();

            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();


            if(!ModelState.IsValid)
            {
                return View();
            }
            bool result=await _context.Categories.AnyAsync(c => c.Name.Trim()==category.Name.Trim() && c.Id!=id);
            if(result)
            {
               ModelState.AddModelError(nameof(category.Name), "Category already exists");
                return View();

            }

            existed.Name = category.Name;
            _context.Categories.Update(category);
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Category category=await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
            if (category == null) return NotFound();
            category.IsDeleted = true;
             _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
