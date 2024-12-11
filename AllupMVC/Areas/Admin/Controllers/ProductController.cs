using AllupMVC.Areas.Admin.ViewModels;
using AllupMVC.DAL;
using AllupMVC.Models;
using AllupMVC.Utilities.Enums;
using AllupMVC.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin,Moderator")]
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
                .Select( p=> 
                new GetProductVM
                {
                    Name = p.Name,
                    Id = p.Id,
                    Price = p.Price,
                    CategoryName=p.Category.Name,
                    BrandName=p.Brand.Name,
                    Image=p.ProductImages.FirstOrDefault(p=>p.IsPrimary==true).Image   
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

            if (!productVM.MainPhoto.ValidateType("image/"))
            {
                ModelState.AddModelError("MainPhoto", "File type is incorrect");
                return View(productVM);
            }


            if (!productVM.MainPhoto.ValidateSize(FileSize.MB, 1))
            {
                ModelState.AddModelError("MainPhoto", "File size is incorrect");
                return View(productVM);
            }


            if (!productVM.HoverPhoto.ValidateType("image/"))
            {
                ModelState.AddModelError("HoverPhoto", "File type is incorrect");
                return View(productVM);
            }


            if (!productVM.HoverPhoto.ValidateSize(FileSize.MB, 1))
            {
                ModelState.AddModelError("HoverPhoto", "File size is incorrect");
                return View(productVM);
            }


            bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError(nameof(CreateProductVM.CategoryId), "Category doesn't exist");
                return View(productVM);
            }
            bool result2 = productVM.Brands.Any(c => c.Id == productVM.BrandId);
            if (!result2)
            {
                ModelState.AddModelError(nameof(CreateProductVM.BrandId), "Brand doesn't exist");
                return View(productVM);
            }


            ProductImage main = new()
            {
                Image = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images"),
                IsPrimary = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false


            };

            ProductImage hover = new()
            {
                Image = await productVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images"),
                IsPrimary = false,
                CreatedAt = DateTime.Now,
                IsDeleted = false


            };

            Product product = new()
            {
                Name = productVM.Name,
                SKU = productVM.SKU,
                CategoryId = productVM.CategoryId.Value,
                BrandId = productVM.BrandId.Value,
                Description = productVM.Description,
                Price = productVM.Price.Value,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                ProductImages = new List<ProductImage> { main, hover }

            };

          


            if (productVM.AdditionalPhotos is not null)
            {
                string text = string.Empty;
                foreach (IFormFile file in productVM.AdditionalPhotos)
                {
                    if (!file.ValidateType("image/"))
                    {
                        text += $"<p class=\"text-warning\">{file.FileName} type was not correct</p>";
                        continue;
                    }
                    if (!file.ValidateSize(FileSize.MB, 1))
                    {
                        text += $"<p class=\"text-warning\">{file.FileName} size was not correct</p>";
                        continue;
                    }

                    product.ProductImages.Add(new ProductImage
                    {
                        Image = await file.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images"),
                        CreatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsPrimary = null
                    });

                }

                TempData["Filewarning"] = text;
            }


            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Product product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            UpdateProductVM productVM = new()
            {
                Name = product.Name,
                SKU = product.SKU,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                Description = product.Description,
                Price = product.Price,
               
                ProductImages = product.ProductImages,
                Categories = await _context.Categories.ToListAsync(),
                Brands = await _context.Brands.ToListAsync()

            };

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateProductVM productVM)
        {
            if (id == null || id < 1) return BadRequest();

            Product existed = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);

            productVM.Categories = await _context.Categories.ToListAsync();
            productVM.Brands = await _context.Brands.ToListAsync();

            productVM.ProductImages = existed.ProductImages;

            if (!ModelState.IsValid)
            {
                return View(productVM);
            }



            if (existed == null) return NotFound();

            if (productVM.MainPhoto != null)
            {
                if (!productVM.MainPhoto.ValidateType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "File type is incorrect");
                    return View(productVM);
                }


                if (!productVM.MainPhoto.ValidateSize(FileSize.MB, 1))
                {
                    ModelState.AddModelError("MainPhoto", "File size is incorrect");
                    return View(productVM);
                }

            }
            if (productVM.HoverPhoto != null)
            {
                if (!productVM.HoverPhoto.ValidateType("image/"))
                {
                    ModelState.AddModelError("HoverPhoto", "File type is incorrect");
                    return View(productVM);
                }


                if (!productVM.HoverPhoto.ValidateSize(FileSize.MB, 1))
                {
                    ModelState.AddModelError("HoverPhoto", "File size is incorrect");
                    return View(productVM);
                }

            }



            if (existed.CategoryId != productVM.CategoryId)
            {
                bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);
                if (!result)
                {
                    return View(productVM);
                }
            }
            if (existed.BrandId != productVM.BrandId)
            {
                bool result = productVM.Brands.Any(c => c.Id == productVM.BrandId);
                if (!result)
                {
                    return View(productVM);
                }
            }

            if (productVM.MainPhoto is not null)
            {
                string fileName = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");


                ProductImage main = existed.ProductImages.FirstOrDefault(p => p.IsPrimary == true);
                main.Image.DeleteFile("");
                existed.ProductImages.Remove(main);
                existed.ProductImages.Add(new ProductImage
                {
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsPrimary = true,
                    Image = fileName
                });
            }

            if (productVM.HoverPhoto is not null)
            {
                string fileName = await productVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");


                ProductImage hover = existed.ProductImages.FirstOrDefault(p => p.IsPrimary == false);
                hover.Image.DeleteFile("");
                existed.ProductImages.Remove(hover);
                existed.ProductImages.Add(new ProductImage
                {
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsPrimary = false,
                    Image = fileName
                });
            }




            if (productVM.ProductImages is null)
            {
                productVM.ImageIds = new List<int>();
            }

            var deletedImages = existed.ProductImages.Where(pi => !productVM.ImageIds.Exists(imgId => imgId == pi.Id) && pi.IsPrimary == null).ToList();
            deletedImages.ForEach(di => di.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images"));


            _context.ProductImages.RemoveRange(deletedImages);


            if (productVM.AdditionalPhotos is not null)
            {
                string text = string.Empty;
                foreach (IFormFile file in productVM.AdditionalPhotos)
                {
                    if (!file.ValidateType("image/"))
                    {
                        text += $"<p class=\"text-warning\">{file.FileName} type was not correct</p>";
                        continue;
                    }
                    if (!file.ValidateSize(FileSize.MB, 1))
                    {
                        text += $"<p class=\"text-warning\">{file.FileName} size was not correct</p>";
                        continue;
                    }

                    existed.ProductImages.Add(new ProductImage
                    {
                        Image = await file.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images"),
                        CreatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsPrimary = null
                    });

                }

                TempData["Filewarning"] = text;
            }

            existed.SKU = productVM.SKU;
            existed.Price = productVM.Price.Value;
            existed.CategoryId = productVM.CategoryId.Value;
            existed.Description = productVM.Description;
            existed.Name = productVM.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}

