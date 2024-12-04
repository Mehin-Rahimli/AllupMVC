using AllupMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace AllupMVC.Areas.Admin.ViewModels
{
    public class CreateProductVM
    {
        public IFormFile MainPhoto { get; set; }
        public IFormFile HoverPhoto { get; set; }
        public List<IFormFile>? AdditionalPhotos { get; set; }
        public string Name { get; set; }

        [Required]
        public decimal? Price { get; set; }
        
        public string Description { get; set; }
        public string SKU { get; set; }

        [Required]
        public int? CategoryId { get; set; }
        [Required]
        public int? BrandId { get; set; }

        public List<Category>? Categories { get; set; }
        public List<Brand>? Brands { get; set; }

    }
}


